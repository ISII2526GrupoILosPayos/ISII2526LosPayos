using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Return
{
    public class PurchaseProductForReturning_PO : PageObject
    {
        // ---- SELECT page ----
        private readonly By inputProductName = By.Id("inputProductName");
        private readonly By inputQuantity = By.Id("inputQuantity");
        private readonly By inputUserName = By.Id("inputUserName");
        private readonly By btnSearch = By.Id("purchaseProductForReturning");

        private readonly By tableOfReturnedProducts = By.Id("TableOfReturnedProducts");

        private readonly By errorAlertNoProducts = By.CssSelector("div.alert-danger");
        private readonly By backToOrdersButton = By.Id("backToOrdersButton");

        // ---- CREATE page ----
        private readonly By btnContinue = By.Id("continueReturnButton");        // ✅ confirmado
        private readonly By selectReturningOption = By.CssSelector("select");   // ✅ solo hay un select
        private readonly By inputRating = By.Id("Rating");                     // ✅ confirmado
        private readonly By btnSaveReturn = By.Id("SubmitReturnOrder");         // ✅ confirmado

        public PurchaseProductForReturning_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }

        // =============== NAVIGATION ===============
        public void GoToSelectPage(string baseUri)
            => _driver.Navigate().GoToUrl(baseUri + "returnorder/purchaseproductforreturning");

        public void WaitForSelectPageLoaded()
        {
            WaitForBeingVisible(inputUserName);
            WaitForBeingClickable(btnSearch);
        }

        public void WaitForDetailsPage()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(d => d.Url.Contains("/returnorder/details"));
        }

        public bool IsOnSelectProductsForPurchasePage()
            => _driver.Url.Contains("/purchaseorder/selectproductsforpurchase");

        // =============== SELECT ACTIONS ===============
        public void FilterProducts(string productName, string userName, int quantity = 1)
        {
            WaitForBeingVisible(inputUserName);

            var prod = _driver.FindElement(inputProductName);
            prod.Clear();
            prod.SendKeys(productName);

            var q = _driver.FindElement(inputQuantity);
            q.Clear();
            q.SendKeys(quantity.ToString());

            var u = _driver.FindElement(inputUserName);
            u.Clear();
            u.SendKeys(userName);

            _driver.FindElement(btnSearch).Click();

            // Espera “real” a que algo cambie: tabla o error o "no products"
            WaitForSelectResultsToLoad();
        }

        private void WaitForSelectResultsToLoad()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try
                {
                    // si aparece el error
                    var alerts = d.FindElements(errorAlertNoProducts);
                    if (alerts.Count > 0 && alerts[0].Displayed) return true;

                    // si aparece el texto de "no products"
                    if (d.PageSource.Contains("No products to show")) return true;

                    // si existe la tabla y tiene al menos 1 fila en tbody
                    var tables = d.FindElements(tableOfReturnedProducts);
                    if (tables.Count > 0)
                    {
                        var rows = d.FindElements(By.CssSelector("#TableOfReturnedProducts tbody tr"));
                        if (rows.Count > 0) return true;
                    }
                }
                catch { }
                return false;
            });
        }

        public bool CheckListOfPurchasedProductsForReturning(System.Collections.Generic.List<string[]> expectedProductsForReturning)
        {
            // Usa el helper del PageObject base (mismo estilo que tu profesora)
            return CheckBodyTable(expectedProductsForReturning, tableOfReturnedProducts);
        }

        public void AddProductByName(string productName)
        {
            var byAddButton = By.XPath($"//tr[td[normalize-space()='{productName}']]//button[normalize-space()='Add']");
            WaitForBeingClickable(byAddButton);
            _driver.FindElement(byAddButton).Click();
        }

        public void ContinueWithReturn()
        {
            WaitForBeingClickable(btnContinue);
            _driver.FindElement(btnContinue).Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(d => d.Url.Contains("/returnorder/createreturnpurchaseorder"));
        }

        // ✅ Back to orders
        public bool CheckBackToOrdersButtonVisible()
        {
            try
            {
                WaitForBeingVisible(backToOrdersButton);
                return _driver.FindElement(backToOrdersButton).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void ClickBackToOrdersAndWait()
        {
            WaitForBeingClickable(backToOrdersButton);
            _driver.FindElement(backToOrdersButton).Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/purchaseorder/selectproductsforpurchase"));
        }

        // =============== CREATE ACTIONS ===============
        public void FillCreateReturnInfo(string returningOptionText, string ratingValue)
        {
            WaitForBeingVisible(selectReturningOption);
            WaitForBeingVisible(inputRating);

            var sel = new SelectElement(_driver.FindElement(selectReturningOption));
            sel.SelectByText(returningOptionText);

            var rating = _driver.FindElement(inputRating);
            rating.Clear();
            rating.SendKeys(ratingValue);
        }

        public void FillReasonForProduct(string productName, string reasonText)
        {
            // buscamos la fila del producto y dentro el input reason
            var rowBy = By.XPath($"//tr[td[normalize-space()='{productName}']]");
            WaitForBeingVisible(rowBy);

            var row = _driver.FindElement(rowBy);

            // tu HTML confirma: name="item.Reason" + id="reason_{productId}_{purchaseOrderId}"
            IWebElement reasonInput = row.FindElement(By.CssSelector("input[name='item.Reason']"));

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(_ => reasonInput.Displayed && reasonInput.Enabled);

            reasonInput.Clear();
            reasonInput.SendKeys(reasonText);
        }

        public void SaveReturn()
        {
            WaitForBeingClickable(btnSaveReturn);
            _driver.FindElement(btnSaveReturn).Click();
        }

        // =============== ASSERTS ===============
        public bool CheckDetailsContains(string returningOption, string productName)
        {
            return _driver.PageSource.Contains("Return Purchase Order Details")
                && _driver.PageSource.Contains("Returning option selected")
                && _driver.PageSource.Contains(returningOption)
                && _driver.PageSource.Contains(productName);
        }

        public bool CheckNoProductsAvailableMessage(string expectedMessage)
        {
            try
            {
                WaitForBeingVisible(errorAlertNoProducts);
                var text = _driver.FindElement(errorAlertNoProducts).Text.Trim();
                return text.Contains(expectedMessage);
            }
            catch
            {
                return false;
            }
        }

        public bool CheckNoProductsToShow()
        {
            return _driver.PageSource.Contains("No products to show");
        }
    }
}
