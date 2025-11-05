using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForMovies.UT;

namespace AppForSEII2526.UT.PurchaseOrdersController_test
{
    public class GetPurchaseOrder_test : AppForSEII25264SqliteUT
    {
        public GetPurchaseOrder_test()
        {
            var brands = new List<Brand>()
            {
                new Brand("Bezoya","Palencia"),
                new Brand("Nike", "Madrid"),
            };

            var products = new List<Product>()
            {
                new Product(1, "Water", "For hydratate yourself", "Blue", 1, 100, brands[0]),
                new Product(2, "Hoodie", "For warming yourself", "Red", 69,100, brands[1]),
                new Product(3, "Shoes", "For jumping too high", "Red", 100,50, brands[1]),
            };

            ApplicationUser user = new ApplicationUser("1", "Luis", "Melero Jareño", "Cueva de Montesinos");

            var purchaseOrder = new PurchaseOrder("Luis Melero Jareño", user, "Cueva de Montesinos", DateTime.Now, new List<PurchaseProduct>());
            
            _context.AddRange(brands);
            _context.AddRange(products);
            _context.SaveChanges();
        }
    }
}
