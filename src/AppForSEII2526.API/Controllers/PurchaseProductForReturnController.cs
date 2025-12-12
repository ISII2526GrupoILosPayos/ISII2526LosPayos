using AppForSEII2526.API.DTOs.ReturnProductDTOs;
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
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchaseProductForReturnController> _logger;

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

            // 1) Proyección a anónimo (100% traducible a SQL)
            var data = await _context.PurchaseProducts
                .AsNoTracking()
                .Include(pp => pp.Product).ThenInclude(p => p.Brand)
                .Include(pp => pp.PurchaseOrder).ThenInclude(po => po.ApplicationUser)
                .Where(pp =>
                    (string.IsNullOrEmpty(productName) || pp.Product.Name.Contains(productName)) &&
                    pp.PurchaseOrder.ApplicationUser.UserName == userName &&
                    pp.ReturnProduct == null &&
                    pp.Quantity >= quantity
                )
                .OrderBy(pp => pp.Product.Name)
                .Select(pp => new
                {
                    pp.ProductId,
                    pp.PurchaseOrderId,
                    Name = pp.Product.Name,
                    Brand = pp.Product.Brand.Name,
                    Quantity = pp.Quantity,
                    Location = pp.Product.Brand.Location,
                    IsReturnable = pp.Product.IsReturnable
                })
                .ToListAsync();

            // 2) Construcción del DTO en memoria (ya NO hay traducción SQL)
            IList<PurchaseProductForReturnDTO> purchaseProductForReturnDTOs = data
                .Select(x => new PurchaseProductForReturnDTO(
                    id: x.ProductId,            // si quieres que Id sea el ProductId
                    name: x.Name,
                    brand: x.Brand,
                    quantity: x.Quantity,
                    location: x.Location,
                    returnable: x.IsReturnable,
                    productid: x.ProductId
                )
                {
                    PurchaseOrderId = x.PurchaseOrderId
                })
                .ToList();

            return Ok(purchaseProductForReturnDTOs);
        }
    }
}
