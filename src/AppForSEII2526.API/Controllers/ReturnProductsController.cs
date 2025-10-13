using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnProductsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ReturnProductsController> _logger;

        public ReturnProductsController(ApplicationDbContext context, ILogger<ReturnProductsController> logger)
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


    }
}
