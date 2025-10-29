using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForMovies.UT;

namespace AppForSEII2526.UT.ProductsController_test
{
    public class GetProductsForPurchase_test : AppForSEII25264SqliteUT
    {
        public GetProductsForPurchase_test()
        {
            var brands = new List<Brand>()
            {
                new Brand("Bezoya","Palencia"),
                new Brand("Nike", "Madrid"),
            };

            var products = new List<Product>()
            {
                new Product(27, "Water", "For hydratate yourself", "Blue", 1, 100),
                new Product(28, "Hoodie", "For warming yourself", "Red", 69,100),
                new Product(29, "Shoes", "For jumping too high", "White", 100,50),
            };
            _context.AddRange(brands);
            _context.AddRange(products);
            _context.SaveChanges();
        }
        [Fact]
        public async Task GetProductsForPurchase_null_name_colour()
        {
            
        }
    }
}
