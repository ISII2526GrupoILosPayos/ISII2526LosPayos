using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Return
{
    internal class CreateReturnPurchaseOrder_PO : PageObject
    {
        // ---- CREATE page ----
        private readonly By btnModifySelectedProducts = By.Id("ModifyProducts");

        public CreateReturnPurchaseOrder_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }

        public void WaitForCreatePageLoaded()
        {
            WaitForBeingVisible(btnModifySelectedProducts);
            WaitForBeingClickable(btnModifySelectedProducts);
        }

        public void ClickModifySelectedProductsAndWaitToSelect()
        {
            WaitForBeingClickable(btnModifySelectedProducts);
            _driver.FindElement(btnModifySelectedProducts).Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/returnorder/purchaseproductforreturning"));
        }
    }
}
