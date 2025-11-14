using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using Humanizer.Localisation;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.UT.PurchaseOrdersController_test
{
    public class CreatePurchaseOrder_test : AppForSEII25264SqliteUT
    {
        private const string _userName = "luis@uclm.es";
        private const string _customerNameSurname = "Luis Melero";
        private const string _street = "Cueva de Montesinos";
        private const string _city = "Villarrobledo";
        private const string _postalCode = "02600";

        private const string _product1Name = "Water";
        private const string _product1Brand = "Bezoya";
        private const string _product2Name = "Hoodie";
        private const string _product2Brand = "Nike";

        private int _paymentMethodId;
        private int _product1Id;
        private decimal _product1Price;

        public CreatePurchaseOrder_test()
        {
            var brands = new List<Brand>()
            {
                new Brand(_product1Brand,"Palencia"),
                new Brand(_product2Brand, "Madrid"),
            };

            var products = new List<Product>()
            {
                new Product(_product1Name, "For hydratate yourself", "Blue", 1, 100, brands[0]),
                new Product(_product2Name, "For warming yourself", "Red", 69, 100, brands[1]),
            };

            ApplicationUser user = new ApplicationUser("1", "Luis", "Melero Jareño", _street);
            user.UserName = _userName;

            var paymentMethod = new Bizum(666666666)
            {
                User = user
            };
            

            var purchaseOrder = new PurchaseOrder(_customerNameSurname, user, _street, _city, _postalCode, DateTime.Now, paymentMethod, new List<PurchaseProduct>());

            purchaseOrder.Products.Add(new PurchaseProduct(products[0], products[0].ProductId, purchaseOrder, 1, products[0].Price));
            purchaseOrder.Products.Add(new PurchaseProduct(products[1], products[1].ProductId, purchaseOrder, 1, products[1].Price));

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.Add(user);
            _context.Add(paymentMethod);
            _context.Add(purchaseOrder);
            _context.SaveChanges();

            _paymentMethodId = paymentMethod.Id;
            _product1Id = products[0].ProductId;
            _product1Price = products[0].Price;
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchaseOrder()
        {
            var purchaseNoProducts = new PurchaseOrderForCreateDTO(_userName, _customerNameSurname, _street, _city, _postalCode, 1, new List<PurchaseProductForCreateDTO>());

            var purchaseProducts = new List<PurchaseProductForCreateDTO>() { new PurchaseProductForCreateDTO(_product2Name, 1) };

            var purchaseUserNotFound = new PurchaseOrderForCreateDTO("paco@gmail.com", _customerNameSurname, _street, _city, _postalCode, 1, purchaseProducts);

            var purchaseMissingStreetField = new PurchaseOrderForCreateDTO(_userName, _customerNameSurname, "", _city, _postalCode, 1, purchaseProducts);

            var purchasePaymentMethodNotFound = new PurchaseOrderForCreateDTO(_userName, _customerNameSurname, _street, _city, _postalCode, 27, purchaseProducts);

            var purchaseProductNotFound = new PurchaseOrderForCreateDTO(_userName, _customerNameSurname, _street, _city, _postalCode, 1, new List<PurchaseProductForCreateDTO>() { new PurchaseProductForCreateDTO { Name = "Fantasma", Quantity = 1 } });

            var purchaseNotEnoughStock = new PurchaseOrderForCreateDTO(_userName, _customerNameSurname, _street, _city, _postalCode, 1, new List<PurchaseProductForCreateDTO>() { new PurchaseProductForCreateDTO { Name = _product1Name, Quantity = 500 } });

            var allTests = new List<object[]>
            {
                new object[] { purchaseNoProducts, "Error! You must include at least one product to be purchased" },
                new object[] { purchaseUserNotFound, "Error! UserName is not registered" },
                new object[] { purchaseMissingStreetField, "Error! All delivery address fields are mandatory (street, city and postal code)" },
                new object[] { purchasePaymentMethodNotFound, "Error! The selected payment method does not exist." },
                new object[] { purchaseProductNotFound, "Error! Product 'Fantasma' not found." },
                new object[] { purchaseNotEnoughStock, "Error! Product 'Water' does not have enough stock (Available: 100)." }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePurchaseOrder))]
        public async Task CreatePurchaseOrder_Error_test(PurchaseOrderForCreateDTO purchaseOrderDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<PurchaseOrdersController>>();
            ILogger<PurchaseOrdersController> logger = mock.Object;

            var controller = new PurchaseOrdersController(_context, logger);

            // Act
            var result = await controller.CreatePurchaseOrder(purchaseOrderDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreatePurchaseOrderl_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchaseOrdersController>>();
            ILogger<PurchaseOrdersController> logger = mock.Object;

            var controller = new PurchaseOrdersController(_context, logger);

            

            var purchaseOrderDTO = new PurchaseOrderForCreateDTO(_userName, _customerNameSurname, _street, _city, _postalCode, _paymentMethodId, new List<PurchaseProductForCreateDTO>() { new PurchaseProductForCreateDTO { Name = _product1Name, Quantity = 1 } });

            var nameParts = _customerNameSurname.Split(' ', 2);
            var expectedName = nameParts[0];
            var expectedSurname = nameParts.Length > 1 ? nameParts[1] : "";

            var expectedrentalDetailDTO = new PurchaseOrderDetailDTO(2, DateTime.Today, expectedName, expectedSurname, _street, _city, _postalCode, 1 * 1, "Bizum", new List<PurchaseProductDTO>() { new PurchaseProductDTO(1, _product1Name, _product1Brand, 1, 1)});

            // Act
            var result = await controller.CreatePurchaseOrder(purchaseOrderDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualRentalDetailDTO = Assert.IsType<PurchaseOrderDetailDTO>(createdResult.Value);

            Assert.Equal(expectedrentalDetailDTO, actualRentalDetailDTO);

        }
    }
}
