using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReturnProductDTOs;
using AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.ReturnPurchaseOrder_test
{
    public class GetReturnPurchaseOrderDetails_test : AppForSEII25264SqliteUT
    {
        private readonly ReturnPurchaseOrder rpo;

        public GetReturnPurchaseOrderDetails_test()
        {
            //
            // 1) Usuario
            //
            var user = new ApplicationUser
            {
                Id = "U1",
                UserName = "pauUser",
                Email = "pau@example.com",
                Name = "Pau",
                Surname = "Femenia",
                Address = "Campus",
                PhoneNumber = "666777888",
                AccountCreationDate = DateTime.Now,
                PurchaseOrders = new List<PurchaseOrder>()
            };

            //
            // 2) Método de pago (PayPal)
            //
            var pm = new PayPal
            {
                Email = "paypal-pau@example.com",
                User = user
            };

            //
            // 3) Marca y producto
            //
            var brand = new Brand
            {
                Id = 10,
                Name = "Zara",
                Location = "Albacete",
                Products = new List<Product>()
            };

            var product = new Product
            {
                ProductId = 1,
                Name = "Movil",
                Description = "Test product for return",
                Colour = "Black",
                Price = 120m,
                Stock = 50,
                IsReturnable = true,
                Brand = brand,
                PurchaseProducts = new List<PurchaseProduct>()
            };

            //
            // 4) Pedido de compra y línea comprada
            //
            var purchaseOrder = new PurchaseOrder
            {
                Id = 1,
                City = "Albacete",
                TotalPrice = 200m,
                Date = DateTime.Now,
                Description = "Pedido de prueba",
                NameSurname = "Pau Femenia",
                PostalCode = "02002",
                Street = "Campus",
                Rating = 5,
                State = PurchaseState.Done,
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                PaymentMethod = pm,
                Products = new List<PurchaseProduct>()
            };

            var purchaseProduct = new PurchaseProduct
            {
                ProductId = product.ProductId,
                Product = product,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseOrder = purchaseOrder,
                Quantity = 2,
                Price = 100m,
                ReturnProduct = null
            };

            purchaseOrder.Products.Add(purchaseProduct);
            product.PurchaseProducts.Add(purchaseProduct);

            //
            // 5) Cabecera de devolución
            //
            rpo = new ReturnPurchaseOrder
            {
                Name = "Devolucion_Prueba",
                TotalPrice = 200m,
                NewTotalPrice = 0m,
                MoneyToReturn = 200m,
                Date = DateTime.Now,
                Customer = user,
                CustomerId = user.Id,
                PaymentMethod = pm,
                ReturnProducts = new List<ReturnProduct>()
            };

            //
            // 6) Producto devuelto (una línea)
            //
            var returnProduct = new ReturnProduct
            {
                Quantity = 2,
                Reason = "Wrong size",
                PurchaseProduct = purchaseProduct,
                ProductId = purchaseProduct.ProductId,
                PurchaseOrderId = purchaseProduct.PurchaseOrderId,
                ReturnOrder = rpo
            };

            rpo.ReturnProducts.Add(returnProduct);

            //
            // 7) Guardamos todo en la BD en memoria
            //
            _context.ApplicationUsers.Add(user);
            _context.PaymentMethods.Add(pm);
            _context.Brands.Add(brand);
            _context.Products.Add(product);
            _context.PurchaseOrders.Add(purchaseOrder);
            _context.PurchaseProducts.Add(purchaseProduct);
            _context.ReturnPurchaseOrders.Add(rpo);
            _context.ReturnProducts.Add(returnProduct);

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

            var expectedProducts = new List<ReturnedProductDTO>
            {
                new ReturnedProductDTO(
                    quantity: 2,
                    productName: "Movil",
                    brandName: "Zara",
                    warehouseLocation: "Albacete"
                )
            };

            var expected = new ReturnPurchaseOrderDTO(
                customerName: "Pau",
                customerFirstSurname: "Femenia",
                customerAddress: "Campus",
                customerTelephoneNumber: "666777888",
                returnedProducts: expectedProducts,
                returningOptionSelected: "PayPal"
            );

            // Act
            var result = await controller.GetReturnPurchaseOrderDetails(rpo.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<ReturnPurchaseOrderDTO>(okResult.Value);

          
            Assert.Equal(expected, actual);
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
