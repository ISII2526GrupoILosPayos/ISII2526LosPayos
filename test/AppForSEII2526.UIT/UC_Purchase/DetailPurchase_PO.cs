using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class DetailPurchase_PO : PageObject
    {
        public DetailPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckPurchaseDetail(string name, string street, string city, string postalCode, string paymentmethod,
            DateTime rentalDate, string totalprice)
        {
            WaitForBeingVisible(By.Id("TotalPrice"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("NameSurname")).Text.Contains(name);
            result = result && _driver.FindElement(By.Id("DeliveryAddress")).Text.Contains(street);
            result = result && _driver.FindElement(By.Id("City")).Text.Contains(city);
            result = result && _driver.FindElement(By.Id("PostalCode")).Text.Contains(postalCode);
            result = result && _driver.FindElement(By.Id("PaymentMethod")).Text.Contains(paymentmethod);
            result = result && _driver.FindElement(By.Id("TotalPrice")).Text.Contains(totalprice);

            var actualRentalDate = DateTime.Parse(_driver.FindElement(By.Id("Date")).Text);
            result = result && ((actualRentalDate - rentalDate) < new TimeSpan(0, 1, 0));

            return result;
            

        }

        public bool CheckListOfProducts(List<string[]> expectedPurchaseProducts)
        {
            return CheckBodyTable(expectedPurchaseProducts, By.Id("PurchasedProducts"));
        }
    }
}
