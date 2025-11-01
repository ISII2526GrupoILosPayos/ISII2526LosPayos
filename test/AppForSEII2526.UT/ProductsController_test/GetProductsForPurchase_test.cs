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
                new Product(1, "Water", "For hydratate yourself", "Blue", 1, 100, brands[0]),
                new Product(2, "Hoodie", "For warming yourself", "Red", 69,100, brands[1]),
                new Product(3, "Shoes", "For jumping too high", "Red", 100,50, brands[1]),
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
                new ProductForPurchaseDTO(2, "Hoodie", "Nike", "Madrid", 100),
                new ProductForPurchaseDTO(3, "Shoes", "Nike", "Madrid", 50),
                new ProductForPurchaseDTO(1, "Water", "Bezoya", "Palencia", 100)
            };
            var mock = new Mock<ILogger<ProductsController>>();
            ILogger<ProductsController> logger = mock.Object;
            ProductsController controller = new ProductsController(_context, logger);

            var result = await controller.GetProductsForPurchase(null, null);

            var okresult=Assert.IsType<OkObjectResult>(result);
            var productsactualresult=Assert.IsType<List<ProductForPurchaseDTO>>(okresult.Value);
            Assert.Equal(expectedProducts, productsactualresult);
        }

        public static IEnumerable<object[]> TestCasesFor_GetProductsForPurchase_OK()
        {
            var productDTOs = new List<ProductForPurchaseDTO>()
            {
                new ProductForPurchaseDTO(1, "Water", "Bezoya", "Palencia",100),
                new ProductForPurchaseDTO(2, "Hoodie", "Nike", "Madrid",100),
                new ProductForPurchaseDTO(3, "Shoes", "Nike", "Madrid",50),
            };
            var productDTOsTC1 = new List<ProductForPurchaseDTO>() { productDTOs[1], productDTOs[2] }
                .OrderBy(p => p.Name).ToList();
            var productDTOsTC2 = new List<ProductForPurchaseDTO>() { productDTOs[1] };
            var productDTOsTC3 = new List<ProductForPurchaseDTO>() { productDTOs[2] };
            var productDTOsTC4 = new List<ProductForPurchaseDTO>() { productDTOs[0], productDTOs[1], productDTOs[2] }
                .OrderBy(p => p.Name).ToList();
            
            var allTests = new List<object[]>
            {
                new object[] { null, "Red", productDTOsTC1 },
                new object[] { "Hoodie", null, productDTOsTC2 },
                new object[] { "S", "Red", productDTOsTC3 },
                new object[] { null, null, productDTOsTC4 },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetProductsForPurchase_OK))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductsForPurchase_OK_test(string? name, string? colour, List<ProductForPurchaseDTO> expectedProducts)
        {
            // Arrange
            var controller = new ProductsController(_context, null);

            // Act
            var result = await controller.GetProductsForPurchase(name, colour);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var productDTOsActual = Assert.IsType<List<ProductForPurchaseDTO>>(okResult.Value);
            Assert.Equal(expectedProducts, productDTOsActual);
        }
    }
}
