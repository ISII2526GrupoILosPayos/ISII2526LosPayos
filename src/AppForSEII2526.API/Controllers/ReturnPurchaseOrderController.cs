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
                          rpo.Id,
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
        [ProducesResponseType(typeof(ReturnPurchaseOrderDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.Conflict)]

public async Task<ActionResult> CreateReturnPurchaseOrder(ReturnPurchaseOrderForCreateDTO model)
{
    //
    // 1. Validaciones básicas
    //

        if (model.Items.Count == 0)
            ModelState.AddModelError(nameof(model.Items),
                "You must include at least one product to return.");


            if (model.Rating == 3)
            ModelState.AddModelError(nameof(model.Rating),
                "Error!, Please, select a value either higher or lower than 3.");


            if (model.Rating.HasValue &&
            (model.Rating.Value < 1 || model.Rating.Value > 5))
            ModelState.AddModelError(nameof(model.Rating),
                "Rating must be between 1 and 5.");

    

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

            // Ejemplo: "RET_20251212163542" (18 chars)
            string baseName = $"RET_{DateTime.Now:yyyyMMddHHmmss}";

            // Validación 10..20
            if (baseName.Length < 10)
            {
                baseName = baseName.PadRight(10, '_');   // rellena con '_' hasta 10
            }
            else if (baseName.Length > 20)
            {
                baseName = baseName.Substring(0, 20);    // recorta a 20
            }


            //
            // 5.2 Creamos la cabecera ReturnPurchaseOrder
            //
            var returnOrder = new ReturnPurchaseOrder
    {
        Name = baseName,

        TotalPrice = totalOriginal,
        NewTotalPrice = newTotalPrice,
        MoneyToReturn = refund,
        Date = DateTime.Now,
        Rating = model.Rating,

        PaymentMethod = selectedPaymentMethod,
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

    //
    // 6. Respuesta 201 - usar ReturnPurchaseOrderDTO + ReturnedProductDTO
    //
    var responseDto = new ReturnPurchaseOrderDTO(
        returnOrder.Id,
        customerName:            user.Name,
        customerFirstSurname:    user.Surname,
        customerAddress:         user.Address,
        customerTelephoneNumber: user.PhoneNumber,
        returnedProducts: returnOrder.ReturnProducts
            .Select(rp => new ReturnedProductDTO(
                quantity:          rp.Quantity,
                productName:       rp.PurchaseProduct.Product.Name,
                brandName:         rp.PurchaseProduct.Product.Brand.Name,
                warehouseLocation: rp.PurchaseProduct.Product.Brand.Location
            ))
            .ToList(),
        returningOptionSelected: returnOrder.PaymentMethod?.GetType().Name
    );

    return CreatedAtAction(
        nameof(GetReturnPurchaseOrderDetails),
        new { id = returnOrder.Id },
        responseDto
    );
}

    }
}
