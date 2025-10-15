using AppForSEII2526.API.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int) HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{
        //    if (op2 == 0)
        //    {
        //        //string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now + "error: " + error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ProductForPurchaseDTO>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult> GetProductsForPurchase(string? productName)
        {
            IList<ProductForPurchaseDTO> productDTOS = await _context.Products
                .Include(product=>product.Brand)        //Cuando quiero hacer una union con otra tabla
                .Where(product=>product.Name.Contains(productName)
                        || (productName==null))
                .OrderBy(product=>product.Name)
                .Select(product=>new ProductForPurchaseDTO(product.ProductId, product.Name, product.Brand.Name))
                .ToListAsync();
            return Ok(productDTOS);
        }
    }
}
