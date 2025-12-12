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

        public SelectProductsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchProducts(string name, string colour)
        {
            WaitForBeingClickable(productName);
            _driver.FindElement(productName).SendKeys(name);
            if (colour == "") colour = "All";
            SelectElement selectElement = new SelectElement(_driver.FindElement(productColour));
            selectElement.SelectByText(colour);
            _driver.FindElement(buttonSearchProducts).Click();
            
        }

        public bool CheckListOfProducts(List<string[]> expectedProducts)
        {

            return CheckBodyTable(expectedProducts, tableOfProductsBy);
        }
    }
}
