using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectProductsForPurchase_PO : PageObject
    {
        By productName = By.Id("inputName");
        By productColour = By.Id("inputColour");
        By buttonSearchProducts = By.Id("searchProducts");
        By tableOfProductsBy = By.Id("TableOfProducts");
        By buttonRentMovies = By.Id("purchaseProductButton");

        public SelectProductsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchProducts(string name, string colour)
        {
            WaitForBeingClickable(productName);
            _driver.FindElement(productName).Clear();
            _driver.FindElement(productName).SendKeys(name);

            _driver.FindElement(productColour).Clear();
            _driver.FindElement(productColour).SendKeys(colour);

            _driver.FindElement(buttonSearchProducts).Click();

        }

        public bool CheckListOfProducts(List<string[]> expectedProducts)
        {

            return CheckBodyTable(expectedProducts, tableOfProductsBy);
        }

        public void AddProductToPurchaseCart(string productName)
        {
            WaitForBeingClickable(By.Id("productToPurchase_" + productName));

            _driver.FindElement(By.Id("productToPurchase_" + productName)).Click();
        }

        public void RemoveProductFromPurchaseCart(string productName)
        {
            WaitForBeingClickable(By.Id("removeProduct_" + productName));
            _driver.FindElement(By.Id("removeProduct_" + productName)).Click();
        }

        public bool PurchaseNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonRentMovies).Displayed == false;
        }
    }
}
