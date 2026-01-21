using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Return
{
    public class PurchaseProductForReturning_PO : PageObject
    {
        
        private readonly By inputProductName = By.Id("inputProductName");
        private readonly By inputQuantity = By.Id("inputQuantity");
        private readonly By inputUserName = By.Id("inputUserName");
        private readonly By btnReturnProduct = By.Id("continueReturnButton");
        //boton de buscar productos
        private readonly By btnSearch = By.Id("purchaseProductForReturning");

        IWebElement _returnButton() => _driver.FindElement(btnReturnProduct);




        private readonly By tableOfReturnedProducts = By.Id("TableOfReturnedProducts");

        private readonly By errorAlertNoProducts = By.CssSelector("div.alert-danger");
        private readonly By backToOrdersButton = By.Id("backToOrdersButton");

        
        private readonly By clearReturnCartButton = By.Id("clearReturnCartButton");

        private readonly By removeProductButtons = By.CssSelector("button[id^='removeProduct_']");



        
        private readonly By btnContinue = By.Id("continueReturnButton");        
        private readonly By selectReturningOption = By.CssSelector("select");   
        private readonly By inputRating = By.Id("Rating");                     
        private readonly By btnSaveReturn = By.Id("SubmitReturnOrder");         

        public PurchaseProductForReturning_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }


        public void SearchProducts(string productName, int quantity, string userName)
        {
            Thread.Sleep(100);
            WaitForBeingClickable(inputProductName);
            Thread.Sleep(100);
            _driver.FindElement(inputProductName).Clear();
            _driver.FindElement(inputProductName).SendKeys(productName);
            Thread.Sleep(100);
            _driver.FindElement(inputQuantity).Clear();
            _driver.FindElement(inputQuantity).SendKeys(quantity.ToString());
            Thread.Sleep(100);
            _driver.FindElement(inputUserName).Clear();
            _driver.FindElement(inputUserName).SendKeys(userName);

            _driver.FindElement(btnSearch).Click();

        }

       

        public void AddProductstoReturnCart(string productName)
        {
            Thread.Sleep(100); 
            WaitForBeingClickable(By.Id("productToReturn_" + productName));
            _driver.FindElement(By.Id("productToReturn_" + productName)).Click();
        }

        public void ReturnProducts()
        {
            WaitForBeingClickable(btnReturnProduct);
            _returnButton().Click();

        }

        public bool NoProductsAvailableMessageIsShown()
        {
            Thread.Sleep(1000);
            return _driver.PageSource.Contains("Errors: The selected order has no products available for returning.");
        }

        public bool CheckBackToOrdersButton()
        {
            Thread.Sleep(100);
             _driver.FindElement(By.Id("backToOrdersButton"));
            return true;
        }

        public bool CheckListOfPurchasedProductsForReturning(List<string[]> expectedProducts)
        {
            return CheckBodyTable(expectedProducts, tableOfReturnedProducts);
        }

        public bool ReturnProductsExpectingNotReturnableError(string productName)
        {
            Thread.Sleep(1000);
            return _driver.PageSource.Contains("Errors: You cannot continue. These products are not returnable:");
        }

        public void EmptyCompleteCart()
        {
                       WaitForBeingClickable(clearReturnCartButton);
            _driver.FindElement(By.Id("clearReturnCartButton")).Click();
        }


        public void RemoveProductFromPurchaseCartByName(string productName)
        {
            var by = By.XPath($"//button[starts-with(@id,'removeProduct_') and contains(normalize-space(.), '{productName}')]");
            WaitForBeingClickable(by);
            _driver.FindElement(by).Click();
        }


        public bool CheckBackToSelectProductsStep2()
        {
            // (Opcional pero recomendado) Espera a que la página esté lista
            WaitForBeingClickable(By.Id("purchaseProductForReturning"));

            return _driver.FindElements(By.Id("inputProductName")).Any()
                && _driver.FindElements(By.Id("inputQuantity")).Any()
                && _driver.FindElements(By.Id("inputUserName")).Any()
                && _driver.FindElements(By.Id("purchaseProductForReturning")).Any();
        }




        /*
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

            
            WaitForSelectResultsToLoad();
        }

        private void WaitForSelectResultsToLoad()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try
                {
                    
                    var alerts = d.FindElements(errorAlertNoProducts);
                    if (alerts.Count > 0 && alerts[0].Displayed) return true;

                    
                    if (d.PageSource.Contains("No products to show")) return true;

                    
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

        
        public void ClickContinueExpectingNotReturnableError(string productName)
        {
            WaitForBeingClickable(btnContinue);
            _driver.FindElement(btnContinue).Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try
                {
                    // seguimos en la misma página (no create)
                    if (d.Url.Contains("/returnorder/createreturnpurchaseorder")) return false;

                    var alerts = d.FindElements(errorAlertNoProducts);
                    if (alerts.Count == 0) return false;

                    var txt = alerts[0].Text ?? "";
                    return alerts[0].Displayed && txt.Contains(productName);
                }
                catch { return false; }
            });
        }

        
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

        
        public bool CheckEmptyCartButtonVisible()
        {
            try
            {
                WaitForBeingVisible(clearReturnCartButton);
                return _driver.FindElement(clearReturnCartButton).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void ClickEmptyCartReturnAndWait()
        {
            WaitForBeingClickable(clearReturnCartButton);
            _driver.FindElement(clearReturnCartButton).Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try
                {
                    
                    var noItems = d.FindElements(By.CssSelector("button[id^='removeProduct_']")).Count == 0;

                    
                    var continueEls = d.FindElements(btnContinue);
                    var continueHidden = continueEls.Count == 0 || !continueEls[0].Displayed;

                    return noItems && continueHidden;
                }
                catch
                {
                    return false;
                }
            });
        }

        public bool IsCartEmpty()
        {
            try
            {
                
                return _driver.FindElements(removeProductButtons).Count == 0;
            }
            catch
            {
                return true;
            }
        }



        public bool IsReturnCartHidden()
        {
            try
            {
                var el = _driver.FindElement(btnContinue);
                return !el.Displayed; 
            }
            catch
            {
                
                return true;
            }
        }

        
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
            
            var rowBy = By.XPath($"//tr[td[normalize-space()='{productName}']]");
            WaitForBeingVisible(rowBy);

            var row = _driver.FindElement(rowBy);

            
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

       
        public bool CheckNotReturnableErrorContains(string expectedPrefix, string productName)
        {
            try
            {
                WaitForBeingVisible(errorAlertNoProducts);
                var text = _driver.FindElement(errorAlertNoProducts).Text.Trim();
                return text.Contains(expectedPrefix) && text.Contains(productName);
            }
            catch
            {
                return false;
            }
        }


        public bool IsOnSelectReturnPage()
    => _driver.Url.Contains("/returnorder/purchaseproductforreturning");



        public bool IsContinueReturnVisible()
        {
            try
            {
                var els = _driver.FindElements(btnContinue);
                return els.Count > 0 && els[0].Displayed;
            }
            catch
            {
                return false;
            }
        }
        */

    }



}

