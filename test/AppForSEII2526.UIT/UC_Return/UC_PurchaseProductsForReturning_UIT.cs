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
        private readonly CreateReturnPurchaseOrder_PO createReturnPurchaseOrder_PO;
        


        private const string userEmail = "pau2@gmail.com";
        private const string password = "Pau123.";

        private const string returningOption = "Bizum";
        private const string rating = "5";
        private const string reason = "I dont Like it";

        // Usuario que ya tiene TODO devuelto
        private const string userEmailAllReturned = "hugo@gmail.com";
        private const string expectedErrorAllReturned =
            "The selected order has no products available for returning.";

        // Mensaje base para NO retornables
        private const string expectedNotReturnablePrefix =
            "You cannot continue. These products are not returnable:";

        public UC_PurchaseProductsForReturning_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
            purchaseProductForReturning_PO = new PurchaseProductForReturning_PO(_driver, _output);
            createReturnPurchaseOrder_PO = new CreateReturnPurchaseOrder_PO(_driver, _output); 
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



        //desde CREATE -> "Modify selected products" -> vuelve a STEP 2 (Select)
        [Theory]
        [InlineData("Calcetines", 5)]
        [InlineData("Sudadera", 3)]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC6_SaveWithoutMandatoryFields_ShowsErrors_AndStaysOnCreate(string productToAdd, int quantityFilter)
        {
            // Arrange
            InitialSteps_GoToSelectReturnProducts();

            // Step 2: seleccionar 1 producto y pasar a Create
            purchaseProductForReturning_PO.FilterProducts(productName: productToAdd, userName: userEmail, quantity: quantityFilter);
            purchaseProductForReturning_PO.AddProductByName(productToAdd);
            purchaseProductForReturning_PO.ContinueWithReturn();

            createReturnPurchaseOrder_PO.WaitForCreatePageLoaded();

            // Act: NO rellenamos ReturningOption ni Reason, click Save
            createReturnPurchaseOrder_PO.ClickSave();

            // Assert: aparecen errores
            createReturnPurchaseOrder_PO.WaitForErrorText("The ReturningOptionSelected field is required.");
            createReturnPurchaseOrder_PO.WaitForErrorText("Reason for returning is required.");

            // (opcional) confirmar que seguimos en Create (Step 4)
            Assert.Contains("/returnorder/createreturnpurchaseorder", _driver.Url);
        }



        //(BASIC FLOW)

        // ✅ ESC6 (AF5): Save sin campos obligatorios -> muestra errores de validación (se queda en Create)
        [Theory]
        [InlineData("Calcetines", 5)]
        [InlineData("Sudadera", 3)]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC6_MandatoryFieldsMissing_ShowsValidationMessages(string productToAdd, int quantityFilter)
        {
            // Arrange
            InitialSteps_GoToSelectReturnProducts();

            // STEP 2 (Select): filtra + Add
            purchaseProductForReturning_PO.FilterProducts(productName: productToAdd, userName: userEmail, quantity: quantityFilter);
            purchaseProductForReturning_PO.AddProductByName(productToAdd);

            // STEP 4 (Create)
            purchaseProductForReturning_PO.ContinueWithReturn();
            createReturnPurchaseOrder_PO.WaitForCreatePageLoaded();

            // Act: Save SIN seleccionar ReturningOptionSelected (y sin tocar reason)
            createReturnPurchaseOrder_PO.ClickSave();

            // Assert: sale el mensaje de ReturningOptionSelected required
            createReturnPurchaseOrder_PO.WaitForValidationMessageContains(
                "The ReturningOptionSelected field is required."
            );

            Assert.True(
                createReturnPurchaseOrder_PO.HasValidationMessageContains("The ReturningOptionSelected field is required."),
                "Error: expected validation message for ReturningOptionSelected was not shown."
            );

            // (Opcional) si también tienes validation-message para Reason, añade:
            // Assert.True(createReturnPurchaseOrder_PO.HasValidationMessageContains("Reason"), "Error: expected reason validation message was not shown.");
        }




        // si no hay productos disponibles para devolver -> error + Back to orders
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


        
        //   continuar devolución hasta Details (THEORY)
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

        // producto NO retornable -> error + Empty Cart/Return
        [Theory]
        [InlineData("Shampoo")]
        [InlineData("Water")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC4_1_2(string notReturnableProduct)
        {
            // Arrange
            InitialSteps_GoToSelectReturnProducts();

            // Act: aseguramos listado "normal" (sin filtrar por quantity exacta del producto)
            // (si filtras por quantity=1 podrías excluir Shampoo(3) o Water(2))
            purchaseProductForReturning_PO.FilterProducts(productName: "", userName: userEmail, quantity: 1);

            // Act: Add producto NO retornable
            purchaseProductForReturning_PO.AddProductByName(notReturnableProduct);

            // Act: Continue (pero debe quedarse en SELECT y mostrar error)
            purchaseProductForReturning_PO.ClickContinueExpectingNotReturnableError(notReturnableProduct);

            // Assert: mensaje contiene prefijo + nombre del producto
            Assert.True(
                purchaseProductForReturning_PO.CheckNotReturnableErrorContains(expectedNotReturnablePrefix, notReturnableProduct),
                $"Error: not returnable message not shown correctly for product {notReturnableProduct}"
            );

            // Act: click Empty Cart/Return
            purchaseProductForReturning_PO.ClickEmptyCartReturnAndWait();

            Assert.True(purchaseProductForReturning_PO.IsOnSelectReturnPage(),
                "Should return to step 2 (select page).");

            Assert.True(purchaseProductForReturning_PO.IsCartEmpty(),
                "Cart should be empty after Empty Cart/Return.");




        }
    }



}