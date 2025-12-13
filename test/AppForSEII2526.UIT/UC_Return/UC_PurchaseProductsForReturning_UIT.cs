using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Return
{
    public class UC_PurchaseProductsForReturning_UIT : UC_UIT
    {
        private readonly PurchaseProductForReturning_PO purchaseProductForReturning_PO;

        private const string userEmail = "pau2@gmail.com";
        private const string password = "Pau123.";

        private const string returningOption = "Bizum";
        private const string rating = "5";
        private const string reason = "I dont Like it";

        // Usuario que ya tiene TODO devuelto
        private const string userEmailAllReturned = "hugo@gmail.com";
        private const string expectedErrorAllReturned =
            "The selected order has no products available for returning.";

        public UC_PurchaseProductsForReturning_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
            purchaseProductForReturning_PO = new PurchaseProductForReturning_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login(userEmail, password);
        }

        private void InitialSteps_GoToSelectReturnProducts()
        {
            Precondition_perform_login();
            purchaseProductForReturning_PO.GoToSelectPage(_URI);
            purchaseProductForReturning_PO.WaitForSelectPageLoaded();
        }

        // ✅ TEST 1 (BASIC FLOW) - Theory para dos productos
        // OJO: este test CREA un return y cambia estado. Debes ejecutarlo con BBDD reseteada/seed consistente.
        [Theory]
        [InlineData("Camiseta")]
        [InlineData("Gorra")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC1_1_2(string productToAdd)
        {
            // Arrange
            InitialSteps_GoToSelectReturnProducts();

            // Act (SELECT): filtra por producto + usuario (quantity base = 1)
            purchaseProductForReturning_PO.FilterProducts(productName: productToAdd, userName: userEmail, quantity: 1);

            // Act: Add producto
            purchaseProductForReturning_PO.AddProductByName(productToAdd);

            // Act: Continue
            purchaseProductForReturning_PO.ContinueWithReturn();

            // Act (CREATE): Bizum + Rating + Reason
            purchaseProductForReturning_PO.FillCreateReturnInfo(returningOption, rating);
            purchaseProductForReturning_PO.FillReasonForProduct(productToAdd, reason);

            // Act: Save
            purchaseProductForReturning_PO.SaveReturn();

            // Assert: Details
            purchaseProductForReturning_PO.WaitForDetailsPage();
            Assert.True(
                purchaseProductForReturning_PO.CheckDetailsContains(returningOption, productToAdd),
                $"Error: details page does not contain expected data for product {productToAdd}"
            );
        }

      

        // ✅ CASO: si no hay productos disponibles para devolver -> error + Back to orders
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC2()
        {
            // Arrange
            InitialSteps_GoToSelectReturnProducts();

            // Act: filtrar por usuario "hugo@gmail.com"
            purchaseProductForReturning_PO.FilterProducts(productName: "", userName: userEmailAllReturned, quantity: 1);

            // Assert: error + “No products to show” + botón Back to orders
            Assert.True(
                purchaseProductForReturning_PO.CheckNoProductsAvailableMessage(expectedErrorAllReturned),
                "Error: expected message was not shown."
            );

            Assert.True(
                purchaseProductForReturning_PO.CheckNoProductsToShow(),
                "Error: expected 'No products to show.' was not shown."
            );

            Assert.True(
                purchaseProductForReturning_PO.CheckBackToOrdersButtonVisible(),
                "Error: 'Back to orders' button is not visible."
            );

            // Act: click Back to orders y esperar navegación
            purchaseProductForReturning_PO.ClickBackToOrdersAndWait();

            // Assert: URL correcta
            Assert.True(
                purchaseProductForReturning_PO.IsOnSelectProductsForPurchasePage(),
                "Error: it did not navigate to /purchaseorder/selectproductsforpurchase after clicking 'Back to orders'."
            );
        }


        // ✅ CASO 2.1-2.3 + continuar devolución hasta Details
        // ✅ CASO 2.1-2.3 + continuar devolución hasta Details (THEORY)
        // Filtra por Name + Quantity, muestra solo los productos que cumplen,
        // y luego sigue el mismo flow que el Basic Flow.
        [Theory]
        [InlineData("Calcetines", 5, "Pull&Bear", "Albacete")]
        [InlineData("Sudadera", 3, "Zara", "Albacete")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC3_1_2(
            string productToAdd,
            int quantityFilter,
            string expectedBrand,
            string expectedWarehouse)
        {
            // Arrange
            InitialSteps_GoToSelectReturnProducts();

            // Act (2.1-2.2): Filtrar por Name + Quantity (+ user obligatorio en tu pantalla)
            purchaseProductForReturning_PO.FilterProducts(productName: productToAdd, userName: userEmail, quantity: quantityFilter);

            // Assert (2.3): Solo aparecen los que cumplen filtros
            var expected = new List<string[]>
    {
        // Columnas: Product | Brand | Quantity Bought | Warehouse
        new string[] { productToAdd, expectedBrand, quantityFilter.ToString(), expectedWarehouse }
    };

            Assert.True(
                purchaseProductForReturning_PO.CheckListOfPurchasedProductsForReturning(expected),
                $"Error: the table does not match expected filtering results for {productToAdd} qty {quantityFilter}."
            );

            // Act: Add producto
            purchaseProductForReturning_PO.AddProductByName(productToAdd);

            // Act: Continue
            purchaseProductForReturning_PO.ContinueWithReturn();

            // Act (CREATE): Bizum + Rating + Reason
            purchaseProductForReturning_PO.FillCreateReturnInfo(returningOption, rating);
            purchaseProductForReturning_PO.FillReasonForProduct(productToAdd, reason);

            // Act: Save
            purchaseProductForReturning_PO.SaveReturn();

            // Assert: Details
            purchaseProductForReturning_PO.WaitForDetailsPage();
            Assert.True(
                purchaseProductForReturning_PO.CheckDetailsContains(returningOption, productToAdd),
                $"Error: details page does not contain expected data after completing AF flow for {productToAdd}."
            );
        }



    }
}
