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
            if (_context.PurchaseOrders == null)
            {
                _logger.LogError("Error: PurchaseOrders table does not exist");
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Where(po => (po.Id == null || po.Id == id))
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
    }
}
