using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchaseOrdersController> _logger;

        public PurchaseOrdersController(ApplicationDbContext context, ILogger<PurchaseOrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseOrderDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders
                .Where(po => po.Id == id)
                .Include(po => po.Products)                 
                    .ThenInclude(pp => pp.Product)          // Join con Product
                        .ThenInclude(p => p.Brand)          // Join con Brand
                .Include(po => po.ApplicationUser)          
                .Select(po => new PurchaseOrderDetailDTO(
                    po.Id,
                    po.Date,
                    po.ApplicationUser.Name, 
                    po.ApplicationUser.Surname,
                    po.Street,
                    po.City,
                    po.PostalCode,
                    po.State,
                    po.TotalPrice,
                    po.Products
                        .Select(pp => new PurchaseProductDTO(
                            pp.Product.ProductId,
                            pp.Product.Name,
                            pp.Product.Brand.Name,
                            pp.Quantity,
                            pp.Price))
                        .ToList()
                ))
                .FirstOrDefaultAsync();

            if (purchaseOrder == null)
            {
                _logger.LogError($"Error: Purchase order with id {id} does not exist");
                return NotFound();
            }

            return Ok(purchaseOrder);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseOrderDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreatePurchaseOrder(PurchaseOrderForCreateDTO purchaseForCreate)
        {
            if (purchaseForCreate.PurchaseProducts == null || purchaseForCreate.PurchaseProducts.Count == 0)
                ModelState.AddModelError("PurchaseProducts", "Error! You must include at least one product to be purchased");

            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == purchaseForCreate.CustomerUserName);
            if (user == null)
                ModelState.AddModelError("PurchaseApplicationUser", "Error! UserName is not registered");

            if (string.IsNullOrEmpty(purchaseForCreate.NameSurname) ||
                string.IsNullOrEmpty(purchaseForCreate.Street) ||
                string.IsNullOrEmpty(purchaseForCreate.City) ||
                string.IsNullOrEmpty(purchaseForCreate.PostalCode))
            {
                ModelState.AddModelError("DeliveryAddress", "Error! All delivery address fields are mandatory (street, city and postal code)");
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var productNames = purchaseForCreate.PurchaseProducts.Select(p => p.Name).ToList();

            var products = _context.Products
                .Include(p => p.Brand)
                .Where(p => productNames.Contains(p.Name))
                .Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Colour,
                    p.Price,
                    p.Brand,
                    p.Stock
                })
                .ToList();

            var paymentMethod = _context.PaymentMethods.FirstOrDefault(pm => pm.Id == purchaseForCreate.PaymentMethodId);

            if (paymentMethod == null)
            {
                ModelState.AddModelError("PaymentMethod", "Error! The selected payment method does not exist.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            PurchaseOrder purchaseOrder = new PurchaseOrder(purchaseForCreate.NameSurname, user, purchaseForCreate.Street, purchaseForCreate.City, purchaseForCreate.PostalCode, DateTime.Today, paymentMethod, new List<PurchaseProduct>());

            purchaseOrder.Rating = purchaseForCreate.Rating ?? 0;

            purchaseOrder.TotalPrice = 0;

            var purchasedProductsDto = new List<PurchaseProductDTO>();
            foreach (var item in purchaseForCreate.PurchaseProducts)
            {
                var product = products.FirstOrDefault(p => p.Name == item.Name);

                if (product == null)
                {
                    ModelState.AddModelError("PurchaseProducts", $"Error! Product '{item.Name}' not found.");
                }
                else if (item.Quantity > product.Stock)
                {
                    ModelState.AddModelError("PurchaseProducts", $"Error! Product '{product.Name}' does not have enough stock (Available: {product.Stock}).");
                }
                else
                {
                    var purchaseProduct = new PurchaseProduct(product.ProductId, purchaseOrder, product.Price, item.Quantity);

                    purchaseOrder.Products.Add(purchaseProduct);

                    // DTO DE SALIDA CORRECTO
                    purchasedProductsDto.Add(new PurchaseProductDTO(
                        product.ProductId,
                        product.Name,
                        product.Brand.Name,
                        item.Quantity,
                        product.Price
                    ));
                }
            }
            purchaseOrder.TotalPrice = purchaseOrder.Products
            .Sum(pp => pp.Price * pp.Quantity);

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Add(purchaseOrder);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("PurchaseOrder", "Error! There was an error while saving your purchase order, please try again later.");
                return Conflict("Error: " + ex.Message);
            }
            
            var parts = purchaseOrder.NameSurname.Split(' ', 2);
            var customerName = parts[0];
            var customerSurname = parts.Length > 1 ? parts[1] : "";

            var purchaseDetail = new PurchaseOrderDetailDTO(
                purchaseOrder.Id,
                purchaseOrder.Date,
                customerName,        // <- ahora bien
                customerSurname,     // <- ahora bien
                purchaseOrder.Street,
                purchaseOrder.City,
                purchaseOrder.PostalCode,
                purchaseOrder.TotalPrice,
                purchaseForCreate.PaymentMethodId,
                purchasedProductsDto
            );


            return CreatedAtAction("GetPurchaseOrder", new { id = purchaseOrder.Id }, purchaseDetail);
            
        }
    }
}
