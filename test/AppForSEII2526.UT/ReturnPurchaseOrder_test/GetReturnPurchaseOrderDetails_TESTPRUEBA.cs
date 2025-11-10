using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReturnProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReturnPurchaseOrder_test
{
    public class GetReturnPurchaseOrderDetails_TESTPRUEBA : AppForSEII25264SqliteUT
    {
        private ReturnPurchaseOrder rpo; // <-- Añade este campo

        public GetReturnPurchaseOrderDetails_TESTPRUEBA()
        {
            var user = new ApplicationUser
            {
                Id = "U1",
                UserName = "pauUser",
                Email = "pau@example.com",
                Name = "Pau",
                Surname = "Femenia",
                Address = "Campus",
                PhoneNumber = "666777888",
                AccountCreationDate = DateTime.Now
            };

            var pm = new PayPal
            {
                User = user,
                Email = "paypal-pau@example.com"
            };

            rpo = new ReturnPurchaseOrder // <-- Asigna a la variable de clase
            {
                Name = "Devolucion_Prueba",
                TotalPrice = 100m,
                NewTotalPrice = 90m,
                MoneyToReturn = 10m,
                Date = DateTime.Now,
                Customer = user,
                CustomerId = user.Id,
                PaymentMethod = pm
            };

            _context.ApplicationUsers.Add(user);
            _context.PaymentMethods.Add(pm);
            _context.ReturnPurchaseOrders.Add(rpo);
            _context.SaveChanges();
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetReturnPurchaseOrder_returns_order_details()
        {
            // Arrange
            var logger = new Mock<ILogger<ReturnPurchaseOrderController>>();
            var controller = new ReturnPurchaseOrderController(_context, logger.Object);

            // Expected result igual que el DTO real (sin PaymentMethod)
            var expected = new ReturnPurchaseOrderDTO(
                customerName: "Pau",
                customerFirstSurname: "Femenia",
                customerAddress: "Campus",
                customerTelephoneNumber: "666777888",
                returnedProducts: new List<ReturnedProductDTO>() // vacío
            );

            // Act
            var result = await controller.GetReturnPurchaseOrderDetails(rpo.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<ReturnPurchaseOrderDTO>(okResult.Value);

            // Comprobamos campo a campo
            Assert.Equal(expected.CustomerName, actual.CustomerName);
            Assert.Equal(expected.CustomerFirstSurname, actual.CustomerFirstSurname);
            Assert.Equal(expected.CustomerAddress, actual.CustomerAddress);
            Assert.Equal(expected.CustomerTelephoneNumber, actual.CustomerTelephoneNumber);

            // Lista vacía porque no hay productos devueltos en el Arrange
            Assert.Empty(actual.ReturnedProducts);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetReturnPurchaseOrder_NotFound_when_id_does_not_exist()
        {
            var logger = new Mock<ILogger<ReturnPurchaseOrderController>>();
            var controller = new ReturnPurchaseOrderController(_context, logger.Object);

            var result = await controller.GetReturnPurchaseOrderDetails(id: 999_999);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No return purchase order found", notFound.Value?.ToString());
        }




    }
}
