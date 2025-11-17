using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReturnProductDTOs;
using AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526.UT.ReturnPurchaseOrder_test
{
    public class CreateReturnPurchaseOrderDetails_test : AppForSEII25264SqliteUT
    {
        private readonly string _userName = "pau@example.com";
        private readonly string _name = "Pau";
        private readonly string _surname = "Femenia";
        private readonly string _address = "Campus";
        private readonly string _phone = "666777888";

        private readonly ReturnPurchaseOrderController _controller;

        public CreateReturnPurchaseOrderDetails_test()
        {
            // === 1️⃣ Crear usuario y método de pago ===
            var user = new ApplicationUser
            {
                Id = "U1",
                UserName = _userName,
                Email = _userName,
                Name = _name,
                Surname = _surname,
                Address = _address,
                PhoneNumber = _phone,
                AccountCreationDate = DateTime.Now
            };

            var pm = new PayPal
            {
                User = user,
                Email = "paypal-pau@example.com"
            };

            // === 2️⃣ Crear datos de compra previos (producto, marca, pedido) ===
            var brand = new Brand
            {
                Id = 1,
                Name = "Brand X",
                Location = "Warehouse 1"
            };

            var product = new Product
            {
                ProductId = 1,
                Name = "Producto de ejemplo",
                Brand = brand,
                Price = 10m,
                IsReturnable = true,
                Description = "Bad",
                Colour = "Blue"
            };

            var purchaseOrder = new PurchaseOrder
            {
                Id = 1,
                ApplicationUser = user,
                Date = DateTime.Now,
                TotalPrice = 20m,
                City = "Albacete",
                NameSurname = "Pau Femenia",
                PostalCode = "03730",
                Street = "Campus",
                PaymentMethod = pm
            };

            var purchaseProduct = new PurchaseProduct
            {
                Product = product,
                ProductId = product.ProductId,
                PurchaseOrder = purchaseOrder,
                PurchaseOrderId = purchaseOrder.Id,
                Quantity = 2,
                Price = 10m
            };

            // === 3️⃣ Añadir todo al contexto ===
            _context.Brands.Add(brand);
            _context.Products.Add(product);
            _context.ApplicationUsers.Add(user);
            _context.PaymentMethods.Add(pm);
            _context.PurchaseOrders.Add(purchaseOrder);

            _context.PurchaseProducts.Add(purchaseProduct);


            _context.SaveChanges();

            var logger = new Mock<ILogger<ReturnPurchaseOrderController>>();
            _controller = new ReturnPurchaseOrderController(_context, logger.Object);
        }

        // ==== 4️⃣ TESTS DE ERROR ====

        public static IEnumerable<object[]> TestCasesFor_CreateReturnPurchaseOrder()
        {
            var validItems = new List<ReturnItemForCreateDTO>
            {
                new ReturnItemForCreateDTO
                {
                    ProductId = 1,
                    PurchaseOrderId = 1,
                    Quantity = 1,
                    Reason = "Producto defectuoso"
                }
            };

            // Sin productos
            var noItems = new ReturnPurchaseOrderForCreateDTO(
                "pau@example.com",
                "PayPal",
                4,
                new List<ReturnItemForCreateDTO>()
            );

            // Usuario no existe
            var userNotFound = new ReturnPurchaseOrderForCreateDTO(
                "fakeuser@example.com",
                "PayPal",
                4,
                validItems
            );

            // Rating fuera de rango
            var invalidRating = new ReturnPurchaseOrderForCreateDTO(
                "pau@example.com",
                "PayPal",
                0,
                validItems
            );


            // Rating fuera de rango
            var ratnigmalExamen = new ReturnPurchaseOrderForCreateDTO(
                "pau@example.com",
                "PayPal",
                3,
                validItems
            );


            return new List<object[]>
            {
                new object[] { noItems, "You must include at least one product to return" },
                new object[] { userNotFound, "User not found." },
                new object[] { invalidRating, "Rating must be between 1 and 5." },
                new object[] { ratnigmalExamen, "Error!, Please, select a value either higher or lower than 3." }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreateReturnPurchaseOrder))]
        public async Task CreateReturnPurchaseOrder_Error_test(ReturnPurchaseOrderForCreateDTO dto, string expectedError)
        {
            var result = await _controller.CreateReturnPurchaseOrder(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            var actualError = problem.Errors.First().Value[0];
            Assert.Contains(expectedError, actualError);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateReturnPurchaseOrder_Success_test()
        {
            // Arrange
            var dto = new ReturnPurchaseOrderForCreateDTO(
                _userName,
                "PayPal",
                5,
                new List<ReturnItemForCreateDTO>
                {
            new ReturnItemForCreateDTO
            {
                ProductId = 1,
                PurchaseOrderId = 1,
                Quantity = 2,
                Reason = "Producto dañado"
            }
                }
            );

            // Act
            var result = await _controller.CreateReturnPurchaseOrder(dto);

            // Assert 1: debe devolver un 201 Created
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);

            // Assert 2: el valor devuelto NO es un DTO, sino un objeto anónimo
            var response = createdAt.Value!;
            var t = response.GetType();

            // --- Leer propiedades del objeto anónimo ---
            string customerName = (string)t.GetProperty("CustomerName")!.GetValue(response)!;
            string customerSurname = (string)t.GetProperty("CustomerSurname")!.GetValue(response)!;
            string customerAddress = (string)t.GetProperty("CustomerAddress")!.GetValue(response)!;
            string customerTelephone = (string)t.GetProperty("CustomerTelephoneNumber")!.GetValue(response)!;
            string returningOption = (string)t.GetProperty("ReturningOptionSelected")!.GetValue(response)!;
            int rating = (int)t.GetProperty("Rating")!.GetValue(response)!;

            var returnedProducts =
                ((IEnumerable<object>)t.GetProperty("ReturnedProducts")!.GetValue(response)!)
                .ToList();

            // --- Comprobaciones equivalentes al ejemplo
            Assert.Equal(_name, customerName);
            Assert.Equal(_surname, customerSurname);
            Assert.Equal(_address, customerAddress);
            Assert.Equal(_phone, customerTelephone);
            Assert.Equal("PayPal", returningOption);
            Assert.Equal(5, rating);

            // --- Validar los productos devueltos ---
            Assert.Single(returnedProducts);

            var rp = returnedProducts.First();
            var tRP = rp.GetType();

            Assert.Equal(2, (int)tRP.GetProperty("Quantity")!.GetValue(rp)!);
            Assert.Equal("Producto de ejemplo", (string)tRP.GetProperty("ProductName")!.GetValue(rp)!);
            Assert.Equal("Brand X", (string)tRP.GetProperty("BrandName")!.GetValue(rp)!);
            Assert.Equal("Warehouse 1", (string)tRP.GetProperty("WarehouseLocation")!.GetValue(rp)!);
            Assert.Equal("Producto dañado", (string)tRP.GetProperty("Reason")!.GetValue(rp)!);
        }


    }
}
