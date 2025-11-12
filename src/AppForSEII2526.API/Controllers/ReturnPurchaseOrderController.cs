using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnPurchaseOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReturnPurchaseOrderController> _logger;

        public ReturnPurchaseOrderController(
            ApplicationDbContext context,
            ILogger<ReturnPurchaseOrderController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReturnPurchaseOrderDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReturnPurchaseOrderDetails(int id)
        {
            var query = _context.ReturnPurchaseOrders
                .Include(rpo => rpo.Customer)
                .Include(rpo => rpo.ReturnProducts)
                    .ThenInclude(rp => rp.PurchaseProduct)
                        .ThenInclude(pp => pp.Product)
                            .ThenInclude(p => p.Brand)
                .Include(rpo => rpo.PaymentMethod)
                .Where(rpo => rpo.Id == id)
                .Select(rpo =>
                    new ReturnPurchaseOrderDTO(
                        rpo.Customer.Name,
                        rpo.Customer.Surname,
                        rpo.Customer.Address,
                        rpo.Customer.PhoneNumber,
                        rpo.ReturnProducts
                            .Select(rp => new ReturnedProductDTO(
                                rp.Quantity,
                                rp.PurchaseProduct.Product.Name,
                                rp.PurchaseProduct.Product.Brand.Name,
                                rp.PurchaseProduct.Product.Brand.Location
                            ))
                            .ToList(),
                        rpo.PaymentMethod.GetType().Name
                    )
                );

            var dto = await query.FirstOrDefaultAsync();

            if (dto == null)
                return NotFound($"No return purchase order found with ID {id}.");

            return Ok(dto);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReturnPurchaseOrderForCreateDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReturnPurchaseOrder(ReturnPurchaseOrderForCreateDTO model)
        {
            //
            // 1. Validaciones básicas
            //
            if (model == null)
            {
                ModelState.AddModelError("Body", "Request body is required.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.CustomerUserName))
                    ModelState.AddModelError(nameof(model.CustomerUserName),
                        "CustomerUserName is required.");

                if (model.Items == null || model.Items.Count == 0)
                    ModelState.AddModelError(nameof(model.Items),
                        "You must include at least one product to return.");

                if (string.IsNullOrWhiteSpace(model.ReturningOptionSelected
))
                    ModelState.AddModelError(nameof(model.ReturningOptionSelected),
                        "PaymentMethod is required (credit card, paypal, other).");

                if (model.Rating.HasValue &&
                    (model.Rating.Value < 1 || model.Rating.Value > 5))
                    ModelState.AddModelError(nameof(model.Rating),
                        "Rating must be between 1 and 5.");

                if (model.Items != null)
                {
                    foreach (var it in model.Items)
                    {
                        if (string.IsNullOrWhiteSpace(it.Reason))
                        {
                            ModelState.AddModelError("Reason",
                                $"Reason is required for product {it.ProductId}.");
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            //
            // 2. Usuario
            //
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == model.CustomerUserName);

            if (user == null)
            {
                ModelState.AddModelError(nameof(model.CustomerUserName),
                    "User not found.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            //
            // 2.5 Método de pago del usuario
            //
            PaymentMethod? selectedPaymentMethod = null;

            var paymentMethodsOfUser = await _context.PaymentMethods
                .Where(pm => pm.User.Id == user.Id)
                .ToListAsync();

            foreach (var pm in paymentMethodsOfUser)
            {
                var pmTypeName = pm.GetType().Name; // "Bizum", "PayPal", etc.
                if (string.Equals(pmTypeName, model.ReturningOptionSelected,
                                  StringComparison.OrdinalIgnoreCase))
                {
                    selectedPaymentMethod = pm;
                    break;
                }
            }

            if (selectedPaymentMethod == null)
            {
                ModelState.AddModelError(nameof(model.ReturningOptionSelected),
                    $"Payment method '{model.ReturningOptionSelected}' is not available for this user.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            //
            // 3. Cargar info de líneas compradas del usuario (proyección segura)
            //
            var purchaseProductsAllFromUser = await _context.PurchaseProducts
                .Where(pp => pp.PurchaseOrder.ApplicationUser.UserName == model.CustomerUserName)
                .Select(pp => new
                {
                    pp.ProductId,
                    pp.PurchaseOrderId,
                    pp.Quantity,
                    pp.Price,
                    pp.ReturnProduct,

                    ProductName = pp.Product.Name,
                    IsReturnable = pp.Product.IsReturnable,
                    BrandName = pp.Product.Brand.Name,
                    BrandLocation = pp.Product.Brand.Location,

                    OrderUserName = pp.PurchaseOrder.ApplicationUser.UserName
                })
                .ToListAsync();

            var purchaseProducts = purchaseProductsAllFromUser
                .Where(ppAnon =>
                    model.Items.Any(i =>
                        i.ProductId == ppAnon.ProductId &&
                        i.PurchaseOrderId == ppAnon.PurchaseOrderId))
                .ToList();

            //
            // 4. Validaciones de negocio
            //
            foreach (var item in model.Items)
            {
                var pp = purchaseProducts.FirstOrDefault(x =>
                    x.ProductId == item.ProductId &&
                    x.PurchaseOrderId == item.PurchaseOrderId);

                if (pp == null)
                {
                    ModelState.AddModelError("ItemNotFound",
                        $"PurchaseProduct not found for ProductId={item.ProductId} and OrderId={item.PurchaseOrderId}.");
                    continue;
                }

                if (pp.OrderUserName != model.CustomerUserName)
                {
                    ModelState.AddModelError("Ownership",
                        $"Order {pp.PurchaseOrderId} does not belong to {model.CustomerUserName}.");
                }

                if (!pp.IsReturnable)
                {
                    ModelState.AddModelError("NotReturnable",
                        $"Product '{pp.ProductName}' is not returnable.");
                }

                if (pp.ReturnProduct != null)
                {
                    ModelState.AddModelError("AlreadyReturned",
                        $"Product '{pp.ProductName}' was already returned.");
                }

                if (item.Quantity < 1 || item.Quantity > pp.Quantity)
                {
                    ModelState.AddModelError("InvalidQuantity",
                        $"Invalid quantity for '{pp.ProductName}'. Max allowed is {pp.Quantity}.");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            //
            // 5. Construcción en memoria de las líneas y cálculo económico
            //
            decimal refund = 0m;
            decimal totalOriginal = 0m;
            var returnProductsList = new List<ReturnProduct>();

            foreach (var item in model.Items)
            {
                var ppEntity = await _context.PurchaseProducts
                    .Include(pp => pp.Product)
                        .ThenInclude(p => p.Brand)
                    .FirstAsync(pp =>
                        pp.ProductId == item.ProductId &&
                        pp.PurchaseOrderId == item.PurchaseOrderId);

                totalOriginal += (ppEntity.Price * ppEntity.Quantity);
                refund += (ppEntity.Price * item.Quantity);

                var rp = new ReturnProduct
                {
                    Quantity = item.Quantity,
                    Reason = item.Reason,

                    ProductId = ppEntity.ProductId,
                    PurchaseOrderId = ppEntity.PurchaseOrderId,

                    PurchaseProduct = ppEntity,
                };

                ppEntity.ReturnProduct = rp;

                returnProductsList.Add(rp);
            }

            decimal newTotalPrice = totalOriginal - refund;
            if (newTotalPrice < 0) newTotalPrice = 0m;

            //
            // 5.1 Generar un Name válido (10..20 caracteres máx)
            //
            string baseName = $"-{user.Name}";

            if (baseName.Length < 10)
            {
                baseName = baseName.PadRight(10, '_'); // rellena con '_' hasta mínimo 10
            }
            else if (baseName.Length > 20)
            {
                baseName = baseName.Substring(0, 20); // recorta a 20
            }

            //
            // 5.2 Creamos la cabecera ReturnPurchaseOrder
            //
            var returnOrder = new ReturnPurchaseOrder
            {
                Name = baseName, // <- AHORA SEGURO QUE CUMPLE [StringLength(20, MinimumLength = 10)]

                TotalPrice = totalOriginal,
                NewTotalPrice = newTotalPrice,
                MoneyToReturn = refund,
                Date = DateTime.Now,
                Rating = model.Rating,

                PaymentMethod = selectedPaymentMethod, // se usa la opción seleccionada por el cliente


                Customer = user,
                CustomerId = user.Id,

                ReturnProducts = returnProductsList
            };

            foreach (var rp in returnProductsList)
            {
                rp.ReturnOrder = returnOrder;
            }

            _context.ReturnPurchaseOrders.Add(returnOrder);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving ReturnPurchaseOrder");

                var rootMsg = ex.InnerException?.Message ?? ex.Message;

                var conflictDetails = new ValidationProblemDetails
                {
                    Title = "Error while saving ReturnPurchaseOrder",
                    Status = StatusCodes.Status409Conflict,
                    Detail = rootMsg,
                };

                return Conflict(conflictDetails);
            }

            // 6. Respuesta 201 (reutilizando los DTOs de creación)
            // 6. Respuesta 201 - Mostrar datos del cliente y de los productos devueltos
            var responseDto = new
            {
                CustomerName = user.Name,
                CustomerSurname = user.Surname,
                CustomerAddress = user.Address,
                CustomerTelephoneNumber = user.PhoneNumber,
                ReturningOptionSelected = returnOrder.PaymentMethod?.GetType().Name,
                Rating = returnOrder.Rating,
                ReturnedProducts = returnOrder.ReturnProducts
                    .Select(rp => new
                    {
                        Quantity = rp.Quantity,
                        ProductName = rp.PurchaseProduct.Product.Name,
                        BrandName = rp.PurchaseProduct.Product.Brand.Name,
                        WarehouseLocation = rp.PurchaseProduct.Product.Brand.Location,
                        Reason = rp.Reason
                    })
                    .ToList()
            };

            return CreatedAtAction(
                nameof(GetReturnPurchaseOrderDetails),
                new { id = returnOrder.Id },
                responseDto
            );


        }
    }
}
