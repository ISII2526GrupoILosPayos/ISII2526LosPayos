using AppForSEII2526.API.DTOs.ReturnProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseProductForReturnController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchaseProductForReturnController> _logger;

        public PurchaseProductForReturnController(ApplicationDbContext context, ILogger<PurchaseProductForReturnController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PurchaseProductForReturnDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetPurchasedProductsForReturning(string? productName, string userName, int quantity)
        {

            if (string.IsNullOrWhiteSpace(userName))
            {
                ModelState.AddModelError(nameof(userName), "UserName is required.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // incluimos Product->Brand y PurchaseOrder->ApplicationUser para poder filtrar por userName
            IList<PurchaseProductForReturnDTO> purchaseProductForReturnDTOs = await _context.PurchaseProducts
                .Include(pp => pp.Product)
                    .ThenInclude(p => p.Brand)
                .Include(pp => pp.PurchaseOrder)
                    .ThenInclude(po => po.ApplicationUser)
                .Where(pp =>
                    (
                        productName == null ||
                        pp.Product.Name.Contains(productName)
                    )
                    && (
                        pp.PurchaseOrder.ApplicationUser.UserName == userName
                        && pp.ReturnProduct == null
                    )
                    && (
                         pp.Quantity >= quantity
                    )
                    //&& (
                        //pp.Product.IsReturnable == true
                   // )
                )
                .OrderBy(pp => pp.Product.Name)
                .Select(pp => new PurchaseProductForReturnDTO(pp.Product.ProductId, pp.Product.Name, pp.Product.Brand.Name, pp.Quantity, pp.Product.Brand.Location)
                {
                    PurchaseOrderId = pp.PurchaseOrderId
                })
                .ToListAsync();


            return Ok(purchaseProductForReturnDTOs);

           
        }
    }

}
