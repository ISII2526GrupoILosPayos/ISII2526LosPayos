using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Return
{
    public class CreateReturnPurchaseOrder_PO : PageObject
    {
        private readonly By btnSaveReturn = By.Id("SubmitReturnOrder");
        private readonly By reasonInputs = By.CssSelector("#TableOfReturnItems input[id^='reason_']");

        private IWebElement ReturningOption() => _driver.FindElement(By.Id("ReturningOptionSelected"));

        public CreateReturnPurchaseOrder_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }

        public void SetReasonForAllItems(string reason)
        {
            WaitForBeingVisible(reasonInputs);

            var inputs = _driver.FindElements(reasonInputs);
            foreach (var input in inputs)
            {
                input.Clear();
                input.SendKeys(reason);
            }
        }

        public void FillCreateReturnInfo(string returningOption, string reason)
        {
            WaitForBeingVisible(By.Id("ReturningOptionSelected"));

            if (!string.IsNullOrEmpty(returningOption))
                new SelectElement(ReturningOption()).SelectByText(returningOption);

            SetReasonForAllItems(reason);
        }

        public void PressReturnYourProducts()
        {
            _driver.FindElement(By.Id("SubmitReturnOrder")).Click();
        }

        public void PressModifyReturnedProducts()
        {
            Thread.Sleep(1000); // Espera para que se actualice la interfaz
            _driver.FindElement(By.Id("ModifyProducts")).Click();
        }

        public bool CheckValidationError(string expectedError)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(driver => driver.PageSource.Contains(expectedError));
            return _driver.PageSource.Contains(expectedError);
        }   



    }
}
