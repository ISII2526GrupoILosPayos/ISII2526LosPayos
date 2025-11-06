using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForMovies.UT;
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;

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

            purchaseOrder.Products.Add(new PurchaseProduct(products[0], purchaseOrder));
            purchaseOrder.Products.Add(new PurchaseProduct(products[1], purchaseOrder));
            purchaseOrder.Products.Add(new PurchaseProduct(products[2], purchaseOrder));

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.AddRange(purchaseOrder);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchaseOrder_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchaseOrdersController>>();
            ILogger<PurchaseOrdersController> logger = mock.Object;

            var controller = new PurchaseOrdersController(_context, logger);

            // Act
            var result = await controller.GetPurchaseOrder(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchaseOrder_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchaseOrdersController>>();
            ILogger<PurchaseOrdersController> logger = mock.Object;
            var controller = new PurchaseOrdersController(_context, logger);


            var expectedPurchaseOrder = new PurchaseOrderDetailDTO(1, DateTime.Now, "Luis", "Melero Jareño",
                        "Avenida Reyes Catolicos","Villarrobledo", "02600", PurchaseState.Done, 170,
                        new List<PurchaseProductDTO>());
            expectedPurchaseOrder.PurchaseProducts.Add(new PurchaseProductDTO(1, "Water", "Bezoya", 1, 1));

            // Act 
            var result = await controller.GetPurchaseOrder(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchaseOrderDTOActual = Assert.IsType<PurchaseOrderDetailDTO>(okResult.Value);
            var eq = expectedPurchaseOrder.Equals(purchaseOrderDTOActual);
            //we check that the expected and actual are the same
            Assert.Equal(expectedPurchaseOrder, purchaseOrderDTOActual);

        }
    }
}
