using AppForSEII2526.API.DTOs.ReturnProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
            // ⚠ IMPORTANTE:
            // - Incluimos Customer
            // - Incluimos ReturnProducts -> PurchaseProduct -> Product -> Brand
            //   porque necesitamos cantidad, productName, brandName, warehouseLocation y reason.

            var query = _context.ReturnPurchaseOrders
                .Include(rpo => rpo.Customer)
                .Include(rpo => rpo.ReturnProducts)
                    .ThenInclude(rp => rp.PurchaseProduct)
                        .ThenInclude(pp => pp.Product)
                            .ThenInclude(p => p.Brand)
                .Where(rpo => rpo.Id == id)
                .Select(rpo =>
                    new ReturnPurchaseOrderDTO(
                        rpo.Customer.Name,
                        rpo.Customer.Surname,
                        rpo.Customer.Address,
                        rpo.Customer.PhoneNumber,
                        rpo.ReturnProducts
                            .Select(rp =>
                                new ReturnedProductDTO(
                                    rp.Quantity,
                                    rp.PurchaseProduct.Product.Name,
                                    rp.PurchaseProduct.Product.Brand.Name,
                                    rp.PurchaseProduct.Product.Brand.Location,
                                    rp.Reason
                                )
                            )
                            .ToList()
                    )
                );

            //  Usamos FirstOrDefaultAsync() en vez de FirstAsync() para NO petar con 500 si no hay resultado
            var dto = await query.FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound($"No return purchase order found with ID {id}.");
            }

            return Ok(dto);
        }
    }
}
