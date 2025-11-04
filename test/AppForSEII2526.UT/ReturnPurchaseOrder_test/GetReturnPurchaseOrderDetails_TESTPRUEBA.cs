using AppForSEII2526.API.Controllers;
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
            //Arrange: usuario
            var logger = new Mock<ILogger<ReturnPurchaseOrderController>>();
            var controller = new ReturnPurchaseOrderController(_context, logger.Object);

            // Act
            var result = await controller.GetReturnPurchaseOrderDetails(rpo.Id);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = ok.Value!;
            var t = dto.GetType();

            // Leemos propiedades por nombre para no depender del namespace/tipo del DTO
            string customerName = (string)t.GetProperty("CustomerName")!.GetValue(dto)!;
            string customerFirstSurname = (string)t.GetProperty("CustomerFirstSurname")!.GetValue(dto)!;
            string customerAddress = (string)t.GetProperty("CustomerAddress")!.GetValue(dto)!;
            string customerTelephone = (string)t.GetProperty("CustomerTelephoneNumber")!.GetValue(dto)!;
            var returnedProducts = (System.Collections.IEnumerable)t.GetProperty("ReturnedProducts")!.GetValue(dto)!;

            Assert.Equal("Pau", customerName);
            Assert.Equal("Femenia", customerFirstSurname);
            Assert.Equal("Campus", customerAddress);
            Assert.Equal("666777888", customerTelephone);

            // Como no añadimos líneas en el Arrange, esperamos 0
            Assert.Empty(returnedProducts.Cast<object>());

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
