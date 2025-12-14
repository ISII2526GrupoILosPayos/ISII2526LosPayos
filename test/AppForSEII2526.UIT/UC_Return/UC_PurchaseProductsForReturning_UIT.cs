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

        
        private const string userEmailAllReturned = "hugo@gmail.com";
        private const string expectedErrorAllReturned =
            "The selected order has no products available for returning.";

        
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



        
        [Theory]
        [InlineData("Calcetines", 5)]
        [InlineData("Sudadera", 3)]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC5_1_2(string productToAdd, int quantityFilter)
        {
            
            InitialSteps_GoToSelectReturnProducts();

            
            purchaseProductForReturning_PO.FilterProducts(productName: productToAdd, userName: userEmail, quantity: quantityFilter);
            purchaseProductForReturning_PO.AddProductByName(productToAdd);

            
            purchaseProductForReturning_PO.ContinueWithReturn();
            createReturnPurchaseOrder_PO.WaitForCreatePageLoaded();

            
            createReturnPurchaseOrder_PO.ClickModifySelectedProductsAndWaitToSelect();

            
            Assert.True(purchaseProductForReturning_PO.IsOnSelectReturnPage(),
                "Error: it should return to Select (step 2) after clicking 'Modify selected products'.");

        }


        

        
        [Theory]
        [InlineData("Calcetines", 5)]
        [InlineData("Sudadera", 3)]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC6_1_2(string productToAdd, int quantityFilter)
        {
            
            InitialSteps_GoToSelectReturnProducts();

           
            purchaseProductForReturning_PO.FilterProducts(productName: productToAdd, userName: userEmail, quantity: quantityFilter);
            purchaseProductForReturning_PO.AddProductByName(productToAdd);

            
            purchaseProductForReturning_PO.ContinueWithReturn();
            createReturnPurchaseOrder_PO.WaitForCreatePageLoaded();

            
            createReturnPurchaseOrder_PO.ClickSave();

            
            createReturnPurchaseOrder_PO.WaitForValidationMessageContains(
                "The ReturningOptionSelected field is required."
            );

            Assert.True(
                createReturnPurchaseOrder_PO.HasValidationMessageContains("The ReturningOptionSelected field is required."),
                "Error: expected validation message for ReturningOptionSelected was not shown."
            );

            // Assert.True(createReturnPurchaseOrder_PO.HasValidationMessageContains("Reason"), "Error: expected reason validation message was not shown.");
        }


        [Theory]
        [InlineData("Camiseta")]
        [InlineData("Gorra")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC1_1_2(string productToAdd)
        {
            
            InitialSteps_GoToSelectReturnProducts();

            
            purchaseProductForReturning_PO.FilterProducts(productName: productToAdd, userName: userEmail, quantity: 1);

            
            purchaseProductForReturning_PO.AddProductByName(productToAdd);

            
            purchaseProductForReturning_PO.ContinueWithReturn();

            
            purchaseProductForReturning_PO.FillCreateReturnInfo(returningOption, rating);
            purchaseProductForReturning_PO.FillReasonForProduct(productToAdd, reason);

            
            purchaseProductForReturning_PO.SaveReturn();

            
            purchaseProductForReturning_PO.WaitForDetailsPage();
            Assert.True(
                purchaseProductForReturning_PO.CheckDetailsContains(returningOption, productToAdd),
                $"Error: details page does not contain expected data for product {productToAdd}"
            );
        }


        
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC2()
        {
            
            InitialSteps_GoToSelectReturnProducts();

            
            purchaseProductForReturning_PO.FilterProducts(productName: "", userName: userEmailAllReturned, quantity: 1);

            
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

            
            purchaseProductForReturning_PO.ClickBackToOrdersAndWait();

            
            Assert.True(
                purchaseProductForReturning_PO.IsOnSelectProductsForPurchasePage(),
                "Error: it did not navigate to /purchaseorder/selectproductsforpurchase after clicking 'Back to orders'."
            );
        }


        
        
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
            
            InitialSteps_GoToSelectReturnProducts();

           
            purchaseProductForReturning_PO.FilterProducts(productName: productToAdd, userName: userEmail, quantity: quantityFilter);

           
            var expected = new List<string[]>
    {
        
        new string[] { productToAdd, expectedBrand, quantityFilter.ToString(), expectedWarehouse }
    };

            Assert.True(
                purchaseProductForReturning_PO.CheckListOfPurchasedProductsForReturning(expected),
                $"Error: the table does not match expected filtering results for {productToAdd} qty {quantityFilter}."
            );

            
            purchaseProductForReturning_PO.AddProductByName(productToAdd);

           
            purchaseProductForReturning_PO.ContinueWithReturn();

            
            purchaseProductForReturning_PO.FillCreateReturnInfo(returningOption, rating);
            purchaseProductForReturning_PO.FillReasonForProduct(productToAdd, reason);

            
            purchaseProductForReturning_PO.SaveReturn();

            
            purchaseProductForReturning_PO.WaitForDetailsPage();
            Assert.True(
                purchaseProductForReturning_PO.CheckDetailsContains(returningOption, productToAdd),
                $"Error: details page does not contain expected data after completing AF flow for {productToAdd}."
            );
        }

        
        [Theory]
        [InlineData("Shampoo")]
        [InlineData("Water")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC4_1_2(string notReturnableProduct)
        {
            
            InitialSteps_GoToSelectReturnProducts();

           
            purchaseProductForReturning_PO.FilterProducts(productName: "", userName: userEmail, quantity: 1);

            
            purchaseProductForReturning_PO.AddProductByName(notReturnableProduct);

            
            purchaseProductForReturning_PO.ClickContinueExpectingNotReturnableError(notReturnableProduct);

            
            Assert.True(
                purchaseProductForReturning_PO.CheckNotReturnableErrorContains(expectedNotReturnablePrefix, notReturnableProduct),
                $"Error: not returnable message not shown correctly for product {notReturnableProduct}"
            );

           
            purchaseProductForReturning_PO.ClickEmptyCartReturnAndWait();

            Assert.True(purchaseProductForReturning_PO.IsOnSelectReturnPage(),
                "Should return to step 2 (select page).");

            Assert.True(purchaseProductForReturning_PO.IsCartEmpty(),
                "Cart should be empty after Empty Cart/Return.");




        }
    }



}