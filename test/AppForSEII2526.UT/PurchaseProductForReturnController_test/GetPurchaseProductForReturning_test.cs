using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReturnProductDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AppForSEII2526.UT;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace AppForSEII2526.UT.PurchaseProductForReturnController_test
{
    public class GetPurchaseProductForReturning_test : AppForSEII25264SqliteUT
    {
        public GetPurchaseProductForReturning_test()
        {
            // 1. Creamos la Brand (firma/sucursal/almacén)
            var brand = new Brand
            {
                Id = 10,
                Name = "Nike",
                Location = "Almacén Central Madrid",
                Products = new List<Product>()

            };

            // 2. Creamos dos productos:
            //    - uno retornable (debe aparecer en la respuesta)
            //    - otro NO retornable (no debe aparecer)
            var returnableProduct = new Product
            {
                ProductId = 1,
                Name = "Zapatilla Roja",
                Description = "Zapatilla deportiva roja",
                Colour = "Rojo",
                Price = 120m,
                Stock = 50,
                IsReturnable = true,
                Brand = brand,
                PurchaseProducts = new List<PurchaseProduct>()
            };

            var nonReturnableProduct = new Product
            {
                ProductId = 2,
                Name = "Sudadera Negra",
                Description = "Sudadera negra con capucha",
                Colour = "Negro",
                Price = 60m,
                Stock = 10,
                IsReturnable = false, // <- importante: este NO debe salir
                Brand = brand,
                PurchaseProducts = new List<PurchaseProduct>()
            };



            // 3. Creamos un usuario con UserName (porque el filtro lo usa)
            var user = new ApplicationUser
            {
                Id = "1",
                UserName = "pauUser",
                Email = "pau@example.com",
                Name = "Pau",
                Surname = "Femenia",
                Address = "Campus",
                PhoneNumber = "666777888",
                AccountCreationDate = DateTime.Now,
                PurchaseOrders = new List<PurchaseOrder>(),
      
            };

            var bizum = new Bizum
            {
                TelephoneNumber = 6542312311,
                User = user
            };

            var paypal = new PayPal
            {
                Email = "pau@gmail.com",
                User = user
            };

            var creditcard = new CreditCard
            {
                CreditCardNumber= "5423413",
                ExpirationDate = DateTime.Now,
                User = user
            };

            // 4. Creamos una PurchaseOrder asociada a ese usuario
            var order = new PurchaseOrder
            {
                Id = 1,
                City = "Albacete",
                TotalPrice = 10,
                Date = DateTime.Now,
                Description = "Muy bien todo",
                NameSurname = "Pau Femenia",
                PostalCode = "02002",
                Street = "Campus",
                Rating = 3,
                State = PurchaseState.Done,
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                PaymentMethod = bizum,
                Products = new List<PurchaseProduct>()
            };

           

            // 5. Creamos las líneas de compra (PurchaseProduct)
            //    Caso válido: cantidad 2, producto retornable, ReturnProduct = null
            var pp1 = new PurchaseProduct
            {
                ProductId = returnableProduct.ProductId,
                Product = returnableProduct,
                PurchaseOrderId = order.Id,
                PurchaseOrder = order,
                Quantity = 2,
                Price = 100m,
                ReturnProduct = null // <- AÚN NO DEVUELTO (esto hace que salga)
            };

            //    Caso inválido: producto NO retornable
            var pp2 = new PurchaseProduct
            {
                ProductId = nonReturnableProduct.ProductId,
                Product = nonReturnableProduct,
                PurchaseOrderId = order.Id,
                PurchaseOrder = order,
                Quantity = 1,
                Price = 200m,
                ReturnProduct = null // pero IsReturnable = false, así que se filtra fuera
            };

            // añadimos a las colecciones navegacionales
            //order.Products.Add(pp1);
            //order.Products.Add(pp2);
            //returnableProduct.PurchaseProducts.Add(pp1);
            //nonReturnableProduct.PurchaseProducts.Add(pp2);

            // 6. Guardamos todo en la BD en memoria
            _context.Brands.Add(brand);
            _context.ApplicationUsers.Add(user);
            _context.Products.AddRange(returnableProduct, nonReturnableProduct);
            _context.PurchaseOrders.Add(order);
            _context.PurchaseProducts.AddRange(pp1, pp2);
            _context.SaveChanges();//Al guardar order me da el error de clave foranea
            
           

        }

        public static IEnumerable<object[]> TestCasesForGetPurchaseProductForReturningTest()
        {
            var PurchaseProduct1 = new PurchaseProductForReturnDTO(
                id: 1,
                name: "Zapatilla Roja",
                brand: "Nike",
                quantity: 2,
                location: "Almacén Central Madrid"
            )
            {
                PurchaseOrderId = 1
            };

            var allTestCases = new List<object[]>
            {
                new object[]
                {
                    null,           // filterProductName
                    "pauUser",      // userName
                    0,              // minQuantity
                    new List<PurchaseProductForReturnDTO> { PurchaseProduct1 } // expected
                },

                new object[]
                {
                    "Zapatilla",    // filterProductName
                    "pauUser",      // userName
                    0,              // minQuantity
                    new List<PurchaseProductForReturnDTO> { PurchaseProduct1 } // expected
                },

                new object[]
                {
                    "Camisa",       // filterProductName
                    "pauUser",      // userName
                    0,              // minQuantity
                    new List<PurchaseProductForReturnDTO>() // expected: vacío
                },
            };

            return allTestCases;
        }

        /*
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchasedProductsForReturning_OK_test()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PurchaseProductForReturnController>>();
            var controller = new PurchaseProductForReturnController(_context, loggerMock.Object);

            string? filterProductName = null;   // sin filtro por nombre
            string userName = "pauUser";        // el mismo UserName que pusimos en ApplicationUser
            int minQuantity = 0;                // queremos todo con Quantity > 0

            // Construimos la lista esperada EXACTAMENTE como la devolvería el controlador
            var expected = new List<PurchaseProductForReturnDTO>()
            {
                new PurchaseProductForReturnDTO(
                    id: 1,                          // pp1.ProductId
                    name: "Zapatilla Roja",         // pp1.Product.Name
                    brand: "Nike",                  // pp1.Product.Brand.Name
                    quantity: 2,                    // pp1.Quantity
                    location: "Almacén Central Madrid" // pp1.Product.Brand.Location
                )
                {
                    PurchaseOrderId = 1            // pp1.PurchaseOrderId
                }
            };

            // Act
            var result = await controller.GetPurchasedProductsForReturning(
                filterProductName,
                userName,
                minQuantity
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualList = Assert.IsType<List<PurchaseProductForReturnDTO>>(okResult.Value);

            // Comparamos usando tu override de Equals en PurchaseProductForReturnDTO
            Assert.Equal(expected, actualList);
        }
        */

        /*
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchasedProductsForReturning_empty_when_minQuantity_too_high()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PurchaseProductForReturnController>>();
            var controller = new PurchaseProductForReturnController(_context, loggerMock.Object);

            string? filterProductName = null;
            string userName = "pauUser";
            int minQuantity = 999; // pedimos más de lo que hay en cualquier línea

            // Act
            var result = await controller.GetPurchasedProductsForReturning(
                filterProductName,
                userName,
                minQuantity
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualList = Assert.IsType<List<PurchaseProductForReturnDTO>>(okResult.Value);

            // esperamos lista vacía
            Assert.Empty(actualList);
        }
        */


        [Theory]
        [MemberData(nameof(TestCasesForGetPurchaseProductForReturningTest))]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchasedProductsForReturning_parametrized_test(string? filterProductName,string userName,int minQuantity
            ,List<PurchaseProductForReturnDTO> expected)
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PurchaseProductForReturnController>>();
            var controller = new PurchaseProductForReturnController(_context, loggerMock.Object);

            // Act
            var result = await controller.GetPurchasedProductsForReturning(
                filterProductName,
                userName,
                minQuantity
            );

            // Assert 1️⃣: Tipo de resultado correcto
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualList = Assert.IsType<List<PurchaseProductForReturnDTO>>(okResult.Value);

            // Assert 2️⃣: Igualdad con el resultado esperado
            Assert.Equal(expected, actualList);

            // Assert 3️⃣ (opcional): Validar coherencia mínima en los datos devueltos
            if (actualList.Count > 0)
            {
                Assert.All(actualList, item =>
                {
                    Assert.False(string.IsNullOrWhiteSpace(item.Name));
                    Assert.False(string.IsNullOrWhiteSpace(item.Brand));
                    Assert.True(item.Quantity > 0);
                    Assert.True(item.PurchaseOrderId > 0);
                });
            }
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchasedProductsForReturning_BadRequest_when_userName_is_null()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchaseProductForReturnController>>();
            ILogger<PurchaseProductForReturnController> logger = mock.Object;
            var controller = new PurchaseProductForReturnController(_context, logger);

            string? filterProductName = null;
            string? userName = null; // ❌ inválido
            int minQuantity = 0;

            // Act
            var result = await controller.GetPurchasedProductsForReturning(
                filterProductName,
                userName,
                minQuantity
            );

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            // extraemos el primer mensaje de error del diccionario de errores
            var problem = problemDetails.Errors.First().Value[0];

            // Comprobamos que el mensaje sea el esperado
            Assert.Equal("UserName is required.", problem);
        }

    }
}
