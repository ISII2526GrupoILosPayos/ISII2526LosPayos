using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectProductsForPurchase_PO : PageObject
    {
        By productName = By.Id("inputName");
        By productColour = By.Id("inputColour");
        By buttonSearchProducts = By.Id("searchProducts");
        public SelectProductsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchProducts(string name)
        {
            WaitForBeingClickable(productName);
            _driver.FindElement(productName).SendKeys(name);
            _driver.FindElement(buttonSearchProducts).Click();
        }
    }
}
