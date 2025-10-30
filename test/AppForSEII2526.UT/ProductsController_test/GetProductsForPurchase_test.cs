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
                new Product(27, "Water", "For hydratate yourself", "Blue", 1, 100, brands[0]),
                new Product(28, "Hoodie", "For warming yourself", "Red", 69,100, brands[1]),
                new Product(29, "Shoes", "For jumping too high", "White", 100,50, brands[1]),
            };
            _context.AddRange(brands);
            _context.AddRange(products);
            _context.SaveChanges();
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductsForPurchase_null_name_colour()
        {
            var expectedProducts = new List<ProductForPurchaseDTO>()
            {
                new ProductForPurchaseDTO(28, "Hoodie", "Nike", "Madrid", 100),
                new ProductForPurchaseDTO(29, "Shoes", "Nike", "Madrid", 50),
                new ProductForPurchaseDTO(27, "Water", "Bezoya", "Palencia", 100)
            };
            var mock = new Mock<ILogger<ProductsController>>();
            ILogger<ProductsController> logger = mock.Object;
            ProductsController controller = new ProductsController(_context, logger);

            var result = await controller.GetProductsForPurchase(null, null);

            var okresult=Assert.IsType<OkObjectResult>(result);
            var productsactualresult=Assert.IsType<List<ProductForPurchaseDTO>>(okresult.Value);
            Assert.Equal(expectedProducts, productsactualresult);
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductsForPurchase_null_filter_name_colour(string? name, string? colour, List<ProductForPurchaseDTO> expectedProducts)
        {
            //var expectedProducts = new List<ProductForPurchaseDTO>()
            //{
            //    new ProductForPurchaseDTO(28, "Hoodie", "Nike", "Madrid", 100),
            //    new ProductForPurchaseDTO(29, "Shoes", "Nike", "Madrid", 50),
            //    new ProductForPurchaseDTO(27, "Water", "Bezoya", "Palencia", 100)
            //};
            var mock = new Mock<ILogger<ProductsController>>();
            ILogger<ProductsController> logger = mock.Object;
            ProductsController controller = new ProductsController(_context, logger);

            var result = await controller.GetProductsForPurchase(name, colour);

            var okresult = Assert.IsType<OkObjectResult>(result);
            var productsactualresult = Assert.IsType<List<ProductForPurchaseDTO>>(okresult.Value);
            Assert.Equal(expectedProducts, productsactualresult);
        }
    }
}
