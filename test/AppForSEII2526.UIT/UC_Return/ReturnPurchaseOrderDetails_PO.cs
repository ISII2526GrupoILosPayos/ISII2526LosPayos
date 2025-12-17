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


        public bool CheckReturnDetails(string nameofUser, string userSurname, string address, int telephone, string returningOption)
        {
            bool result = true;
            result = result && _driver.FindElement(By.Id("CustomerName")).Text.Contains(nameofUser);
            result = result && _driver.FindElement(By.Id("CustomerFirstSurname")).Text.Contains(userSurname);
            result = result && _driver.FindElement(By.Id("CustomerAddress")).Text.Contains(address);
            result = result && _driver.FindElement(By.Id("CustomerTelephoneNumber")).Text.Contains(telephone.ToString());
            result = result && _driver.FindElement(By.Id("ReturningOptionSelected")).Text.Contains(returningOption);
            return result;


        }


    }
}
