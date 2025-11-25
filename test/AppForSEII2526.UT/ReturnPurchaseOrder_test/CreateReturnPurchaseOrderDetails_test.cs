using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

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
            var user = new ApplicationUser
            {
                Id = "U1",
                UserName = _userName,          // 👈 coincide con CustomerUserName del DTO
                Email = _userName,
                Name = _name,
                Surname = _surname,
                Address = _address,
                PhoneNumber = _phone,
                AccountCreationDate = DateTime.Now,
                PurchaseOrders = new List<PurchaseOrder>()
            };

            // === 2️⃣ Método de pago (PayPal) ===
            var pm = new PayPal
            {
                Email = "paypal-pau@example.com",
                User = user
            };

            // === 3️⃣ Marca ===
            var brand = new Brand
            {
                Id = 1,
                Name = "Brand X",
                Location = "Warehouse 1",
                Products = new List<Product>()
            };

            // === 4️⃣ Producto comprado (NOT NULL cubiertos) ===
            var product = new Product
            {
                ProductId = 1,
                Name = "Producto de ejemplo",
                Description = "Bad",          // NOT NULL
                Colour = "Blue",              // NOT NULL
                Price = 10m,                  // NOT NULL
                Stock = 100,                  // NOT NULL
                IsReturnable = true,
                Brand = brand,
                PurchaseProducts = new List<PurchaseProduct>()
            };

            // === 5️⃣ Pedido de compra original ===
            var purchaseOrder = new PurchaseOrder
            {
                Id = 1,
                City = "Albacete",           // NOT NULL
                TotalPrice = 20m,            // NOT NULL
                Date = DateTime.Now,
                Description = "Pedido de prueba",
                NameSurname = "Pau Femenia",
                PostalCode = "03730",
                Street = "Campus",
                Rating = 5,
                State = PurchaseState.Done,
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                PaymentMethod = pm,
                Products = new List<PurchaseProduct>()
            };

            // === 6️⃣ Línea de compra (PurchaseProduct) ===
            var purchaseProduct = new PurchaseProduct
            {
                Product = product,
                ProductId = product.ProductId,
                PurchaseOrder = purchaseOrder,
                PurchaseOrderId = purchaseOrder.Id,
                Quantity = 2,
                Price = 10m,
                ReturnProduct = null
            };

            // === 7️⃣ Navegación coherente ===
            brand.Products.Add(product);
            product.PurchaseProducts.Add(purchaseProduct);
            user.PurchaseOrders.Add(purchaseOrder);
            purchaseOrder.Products.Add(purchaseProduct);

            // === 8️⃣ Guardar en BD en memoria ===
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

        // ==== 9️⃣ Casos de error (BadRequest) ====

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

            // Rating = 3 (caso especial examen)
            var ratingMalExamen = new ReturnPurchaseOrderForCreateDTO(
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
                new object[] { ratingMalExamen, "Error!, Please, select a value either higher or lower than 3." }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateReturnPurchaseOrder))]
        public async Task CreateReturnPurchaseOrder_Error_test(ReturnPurchaseOrderForCreateDTO dto, string expectedError)
        {
            var result = await _controller.CreateReturnPurchaseOrder(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            var actualError = problem.Errors.First().Value[0];
            Assert.Contains(expectedError, actualError);
        }

        // ==== 🔟 Caso de éxito: 201 + ReturnPurchaseOrderDTO completo ====

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReturnPurchaseOrder_Success_test()
        {
            // Arrange: DTO de entrada válido
            var dto = new ReturnPurchaseOrderForCreateDTO(
                _userName,      // 👈 coincide con user.UserName
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

            // Assert 1: 201 Created
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);

            // Assert 2: el Value es un ReturnPurchaseOrderDTO (ya NO es objeto anónimo)
            var actual = Assert.IsType<ReturnPurchaseOrderDTO>(createdAt.Value);

            // Esperado: un solo producto devuelto
            var expectedProducts = new List<ReturnedProductDTO>
            {
                new ReturnedProductDTO(
                    quantity: 2,
                    productName: "Producto de ejemplo",
                    brandName: "Brand X",
                    warehouseLocation: "Warehouse 1"
                )
            };

            var expected = new ReturnPurchaseOrderDTO(
                customerName: _name,
                customerFirstSurname: _surname,
                customerAddress: _address,
                customerTelephoneNumber: _phone,
                returnedProducts: expectedProducts,
                returningOptionSelected: "PayPal"
            );

            // ✅ Igual que con el GET: un único Assert.Equal(expected, actual)
            Assert.Equal(expected, actual);
        }
    }
}
