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
    public class ReturnPurchaseOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReturnPurchaseOrderController> _logger;

        public ReturnPurchaseOrderController(ApplicationDbContext context, ILogger<ReturnPurchaseOrderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ReturnPurchaseOrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetReturnPurchaseOrderDetails(int id)
        {
            // incluimos Customer y ReturnProducts -> PurchaseProduct -> Product -> Brand
            ReturnPurchaseOrderDTO ? returnPurchaseOrderDTOs = await _context.ReturnPurchaseOrders
                .Include(rpo => rpo.Customer)
                //.Include(rpo => rpo.ReturnProducts)
                //    .ThenInclude(rp => rp.PurchaseProduct)
                //        .ThenInclude(pp => pp.Product)
                //            .ThenInclude(p => p.Brand)
                .Where(rpo => rpo.Id == id)
                .Select(rpo => new ReturnPurchaseOrderDTO(rpo.Date,rpo.PaymentMethod,rpo.MoneyToReturn,rpo.Rating,rpo.Name,rpo.Customer.Surname,rpo.Customer.Address,rpo.Customer.PhoneNumber)
                
                {
                        //Name = rpo.Customer.Name,
                        //FirstSurname = rpo.Customer.Surname,
                        //Address = rpo.Customer.Address,
                        //Phone = rpo.Customer.PhoneNumber,
                        //ReturnDate = rpo.Date,
                    //PaymentMethod = rpo.PaymentMethod.,
                        //MoneyToReturn = rpo.MoneyToReturn,
                        //Rating = rpo.Rating,
                    //ReturnedProducts = rpo.ReturnProducts
                    //    .Select(rp => new ReturnedProductDTO
                    //    {
                    //        Quantity = rp.Quantity,
                    //        ProductName = rp.PurchaseProduct.Product.Name,
                    //        BrandName = rp.PurchaseProduct.Product.Brand.Name,
                    //       /// WarehouseLocation = rp.PurchaseProduct.Product.Brand.Location,
                    //        Reason = rp.Reason
                    //    })
                    //    .ToList()
                })
                .FirstAsync();

            return Ok(returnPurchaseOrderDTOs);
        }
    }
}
