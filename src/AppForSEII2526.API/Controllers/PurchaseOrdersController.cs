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
        }
    }
}
