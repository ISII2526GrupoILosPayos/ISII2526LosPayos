using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UC_PurchaseProducts_UIT : UC_UIT
    {
        private SelectProductsForPurchase_PO selectProductsForPurchase_PO;
        private const int productId1 = 1;
        private const string productName1 = "PS5";
        private const string productBrand1 = "Sony";
        private const string productLocation1 = "USA";
        private const string productQuantity1 = "10";

        private const string productName2 = "Pantalon";
        private const string productBrand2 = "Pull&Bear";
        private const string productLocation2 = "Albacete";
        private const string productQuantity2 = "30";

        public UC_PurchaseProducts_UIT(ITestOutputHelper output) : base(output)
        {
            selectProductsForPurchase_PO = new SelectProductsForPurchase_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("Luis.melero1@alu.uclm.es", "Password1234%");
        }

        private void InitialStepsForPurchaseProducts()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectProductsForPurchase_PO.WaitForBeingVisible(By.Id("CreatePurchaseOrder"));
            //we click on the menu
            _driver.FindElement(By.Id("CreatePurchaseOrder")).Click();
        }

        [Theory]
        [InlineData(productName1, productBrand1, productLocation1, productQuantity1, "PS5", "")]
        [InlineData(productName2, productBrand2, productLocation2, productQuantity2, "", "Negro")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC77_AF1_UC77_4_5filtering(string productName, string productBrand, string productLocation, string productQuantity,
            string filterName, string filterColour)
        {
            //Arrange
            InitialStepsForPurchaseProducts();
            var expectedProducts = new List<string[]> { new string[] { productName, productBrand, productLocation, productQuantity }, };

            //Act
            selectProductsForPurchase_PO.SearchProducts(filterName, filterColour);

            //Assert

            Assert.True(selectProductsForPurchase_PO.CheckListOfProducts(expectedProducts));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC77_AF3_UC77_6_RentingNotavailable()
        {
            //Arrange
            InitialStepsForPurchaseProducts();
            //Act

            //We search products for being able to add some products to the cart
            selectProductsForPurchase_PO.SearchProducts("", "");

            selectProductsForPurchase_PO.AddProductToPurchaseCart(productName1);
            selectProductsForPurchase_PO.RemoveProductFromPurchaseCart(productName1);

            //Assert

            Assert.True(selectProductsForPurchase_PO.PurchaseNotAvailable());

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC77_AF4_UC77_7_ModifyPurchaseCart()
        {
            //Arrange
            InitialStepsForPurchaseProducts();
            //Act

            //We search products for being able to add some products to the cart
            selectProductsForPurchase_PO.SearchProducts("", "");

            selectProductsForPurchase_PO.AddProductToPurchaseCart(productName1);
            selectProductsForPurchase_PO.AddProductToPurchaseCart(productName2);
            selectProductsForPurchase_PO.RemoveProductFromPurchaseCart(productName1);

            //Assert

            Assert.True(selectProductsForPurchase_PO.PurchaseAvailable());
        }

        [Theory]
        [InlineData("", "Av. España, 1", "Albacete", "02001", "Bizum", "The NameSurname field is required.")]
        [InlineData("Luis", "Av. España, 1", "Albacete", "02001", "Bizum", "The field NameSurname must be a string with a minimum length of 5 and a maximum length of 50.")]
        [InlineData("Luis Melero Jareño Mbappe Iniesta Messi Bellingham Porras", "Av. España, 1", "Albacete", "02001", "Bizum", "The field NameSurname must be a string with a minimum length of 5 and a maximum length of 50.")]
        [InlineData("Luis Melero", "", "Albacete", "02001", "Bizum", "The Street field is required.")]
        [InlineData("Luis Melero", "Av. España, 1", "", "02001", "Bizum", "The City field is required.")]
        [InlineData("Luis Melero", "Av. España, 1", "Albacete", "", "Bizum", "The PostalCode field is required.")]
        [InlineData("Luis Melero", "Av. España, 1", "Albacete", "1", "Bizum", "The field PostalCode must be a string with a minimum length of 3 and a maximum length of 10.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC77_AF5_UC77_8_9_10_11_13_14_15_testingErrorsMandatorydata(string nameSurname, string deliveryAddress, string city, string postalCode, string paymentMethod, string expectedMessageError)
        {
            var createPurchase_PO = new CreatePurchase_PO(_driver, _output);

            InitialStepsForPurchaseProducts();

            selectProductsForPurchase_PO.SearchProducts("", "");
            selectProductsForPurchase_PO.AddProductToPurchaseCart(productName1);

            selectProductsForPurchase_PO.PurchaseProducts();

            createPurchase_PO.FillInPurchaseInfo(nameSurname, deliveryAddress, city, postalCode, paymentMethod);
            createPurchase_PO.PressRentYourMovies();

            //Assert
            //the expected error is shown in the view
            Assert.True(createPurchase_PO.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        //We add another diferent test for the UC77_12 and UC77_16 because this errors appear after pressing the "Save" button

        [Theory]
        [InlineData("Luis Melero", "Av. España, 1", "Albacete", "02001", "", "Error! The selected payment method does not exist.")]
        [InlineData("Luis Melero", "Av. España 1", "Albacete", "02001", "Bizum", "Error! You must include a comma to separate the street from the number of the house")]
        public void UC77_AF5_UC77_12_16_testingErrorsMandatorydata(string nameSurname, string deliveryAddress, string city, string postalCode, string paymentMethod, string expectedMessageError)
        {
            var createPurchase_PO = new CreatePurchase_PO(_driver, _output);

            InitialStepsForPurchaseProducts();

            selectProductsForPurchase_PO.SearchProducts("", "");
            selectProductsForPurchase_PO.AddProductToPurchaseCart(productName1);

            selectProductsForPurchase_PO.PurchaseProducts();

            createPurchase_PO.FillInPurchaseInfo(nameSurname, deliveryAddress, city, postalCode, paymentMethod);
            createPurchase_PO.PressRentYourMovies();
            createPurchase_PO.ConfirmPurchase();

            //Assert
            //the expected error is shown in the view
            Assert.True(createPurchase_PO.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }
    }
}
