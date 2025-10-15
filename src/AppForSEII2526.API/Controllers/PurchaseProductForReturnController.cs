using AppForSEII2526.API.DTOs.ReturnProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int) HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        //public async Task<ActionResult>  ComputeDivision(decimal op1, decimal op2)
        //{
        //    if (op2== 0)
        //    {
        //        string error = "OP2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now + "Error" + error);
        //        return BadRequest(error);
        //    }
        //   decimal result =  op1/op2;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PurchaseProductForReturnDTO>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult> GetPurchasedProductsForReturning(string ? productName)
        {
            IList<PurchaseProductForReturnDTO> purchaseProductForReturnDTOs = await _context.PurchaseProducts
                .Include(product => product.Product.Brand)
                .Where(product =>product.Product.Name.Contains(productName)
                || (productName == null))
                .OrderBy(product => product.Product.Name)
                .Select(product => new PurchaseProductForReturnDTO(product.Product.ProductId, product.Product.Name, product.Product.Brand.Name))
                .ToListAsync();
            return Ok(purchaseProductForReturnDTOs);
        }


    }
}
