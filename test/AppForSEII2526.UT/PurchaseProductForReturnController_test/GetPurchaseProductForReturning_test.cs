using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReturnProductDTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace AppForSEII2526.UT.PurchaseProductForReturnController_test
{
    public class GetPurchaseProductForReturning_test : AppForSEII25264SqliteUT
    {
        public GetPurchaseProductForReturning_test()
        {
            var purchaseProduct = new List<AppForSEII2526.API.Models.PurchaseProduct>
            {
                new AppForSEII2526.API.Models.PurchaseProduct
                {
                    ProductId = 1,
                    PurchaseOrderId = 1,
                    Price = 100,
                    Quantity = 2
                },
                new AppForSEII2526.API.Models.PurchaseProduct
                {
                    ProductId = 2,
                    PurchaseOrderId = 1,
                    Price = 200,
                    Quantity = 1
                }


            };

            var purchaseOrder = new AppForSEII2526.API.Models.PurchaseOrder
            {
                City = "Albacete",
                TotalPrice = 10,
                Date = DateTime.Now,
                Description = "VeryGood",
                NameSurname = "Fernandez",
                PostalCode = "02002",
                Street = "Campus",
                Rating = 3,
                State = AppForSEII2526.API.Models.PurchaseState.Done,
                ApplicationUserId = "1"
            };

            var user = new AppForSEII2526.API.Models.ApplicationUser
            {
                Name = "Pau",
                Surname = "Femenia",
                Address = "Campus",
                AccountCreationDate = DateTime.Now
            };


            _context.ApplicationUsers.Add(user);
            _context.PurchaseOrders.Add(purchaseOrder);
            _context.PurchaseProducts.AddRange(purchaseProduct);
            _context.SaveChanges();

        }
        [Fact]
        public async Task GetPurchaseProductForReturning_null_PP_PO_AU()
        {
            IList<PurchaseProductForReturnDTO> purchaseproductForReturn = new IList<PurchaseProductForReturnDTO> ()
            {

            }
        }

}
