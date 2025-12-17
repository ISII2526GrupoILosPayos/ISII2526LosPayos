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

        private const string productName1 = "Sudadera";
        private const string productName2 = "Camiseta";
        private const string productName3 = "Shampoo";
        private const string productName4 = "Gorra";


        private const string brandName1 = "Zara";
        private const string brandName2 = "Pull&Bear";
        private const string brandName3 = "H&S";
        private const string brandName4 = "";


        private const int quantity1 = 1;
        private const int quantity2 = 2;
        private const int quantity3 = 10;

        private const string warehouse1 = "Albacete";
        private const string warehouse2 = "Valencia";
        private const string warehouse3 = "Madrid";
       

        private const string userName = "pau2@gmail.com";
        private const string nameofUser = "Pau";
        private const string userSurname = "Femenia";
        private const string address = "Campus";
        private const int telephone = 65432451;
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
        }

        private void Precondition_perform_login()
        {
            Perform_login(userName, password);
        }

        private void InitialSteps_GoToSelectReturnProducts()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            purchaseProductForReturning_PO.WaitForBeingVisible(By.Id("ReturnPurchaseOrder"));
            //we click on the option of the menu
            _driver.FindElement(By.Id("ReturnPurchaseOrder")).Click();

        }

        [Theory]
        [InlineData("Bizum","I dont like it")]
        [InlineData("PayPal", "I dont like it")]
        public void UC37_ESC1_BF_1_2(string returningOption,string reason)
        {
            var createReturnPurchaseOrder_PO = new CreateReturnPurchaseOrder_PO(_driver, _output);
            var returnpurchaseorderDetails_PO = new ReturnPurchaseOrderDetails_PO(_driver, _output);

            InitialSteps_GoToSelectReturnProducts();

            //UserName no es un filtro y Quantity nuestri estandard es == 1
            purchaseProductForReturning_PO.SearchProducts("", 1, userName);
            purchaseProductForReturning_PO.AddProductstoReturnCart(productName1);

            purchaseProductForReturning_PO.ReturnProducts();

            createReturnPurchaseOrder_PO.FillCreateReturnInfo(returningOption, reason);

            createReturnPurchaseOrder_PO.PressReturnYourProducts();
           // createReturnPurchaseOrder_PO.ConfirmReturn();  Nosotros no tenemos confimacion de compra solo boton

            Assert.True(returnpurchaseorderDetails_PO.CheckReturnDetails(nameofUser, userSurname,address, telephone, returningOption),
                $"Error: details page does not contain expected."
            );

            var expectedReturnedProducts = new List<string[]>
            {
                new string[] { quantity1.ToString(), productName1, brandName1,warehouse1}
            };

            Assert.True(returnpurchaseorderDetails_PO.CheckListOfProducts(expectedReturnedProducts),"Error: the returned products list does not match the expected one.");

        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC2_AF1_()
        {

            InitialSteps_GoToSelectReturnProducts();
            purchaseProductForReturning_PO.SearchProducts("", 1, userEmailAllReturned);

            Assert.True(
                purchaseProductForReturning_PO.NoProductsAvailableMessageIsShown(),
                "Error: expected message was not shown:NoProductsAvailableMessageIsShown.");

        }



        [Theory]
        [InlineData(productName3, brandName3, quantity3, warehouse3, "", 10)]
        [InlineData(productName4, brandName1, quantity1, warehouse1, "Gorra", 1)]
        
        [Trait("LevelTesting", "Functional Testing")]
        public void UC37_ESC3_AF2_(string productName, string productBrand, int quantity, string location, string filterProductName,
            int filterProductQuantity)
        {
            //Arrange
            InitialSteps_GoToSelectReturnProducts();
            var expectedProducts = new List<string[]>{new string[] { productName, productBrand, quantity.ToString(), location }};

            //Act
            purchaseProductForReturning_PO.SearchProducts(filterProductName, filterProductQuantity, userName);

            //Assert
            Assert.True(
                purchaseProductForReturning_PO.CheckListOfPurchasedProductsForReturning(expectedProducts)
            );

        }


        /*
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

        */
    }



}