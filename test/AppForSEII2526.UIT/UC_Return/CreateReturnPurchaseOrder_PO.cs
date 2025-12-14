using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Return
{
    internal class CreateReturnPurchaseOrder_PO : PageObject
    {
        
        private readonly By btnModifySelectedProducts = By.Id("ModifyProducts");
        private readonly By validationMessages = By.CssSelector("li.validation-message");
        private readonly By btnSaveReturn = By.Id("SubmitReturnOrder");

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


        public void ClickSave()
        {
            WaitForBeingClickable(btnSaveReturn);
            _driver.FindElement(btnSaveReturn).Click();
        }

        public void WaitForErrorText(string expectedText)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.PageSource.Contains(expectedText));
        }

        public IList<string> GetValidationMessages()
        {
            return _driver.FindElements(validationMessages)
                .Select(e => (e.Text ?? "").Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();
        }

        public void WaitForValidationMessageContains(string expectedText)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(_ => GetValidationMessages().Any(m => m.Contains(expectedText)));
        }

        public bool HasValidationMessageContains(string expectedText)
            => GetValidationMessages().Any(m => m.Contains(expectedText));
    }
}
