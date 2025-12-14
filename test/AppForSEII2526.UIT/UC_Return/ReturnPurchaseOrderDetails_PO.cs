using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Return
{
    public class ReturnPurchaseOrderDetails_PO : PageObject
    {
        public ReturnPurchaseOrderDetails_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) {  }

        public bool IsOnDetailsPage()
            => _driver.Url.Contains("/returnorder/details");

        public void WaitForDetailsPageLoaded()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/returnorder/details"));
        }
    }
}
