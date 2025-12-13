using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class CreatePurchase_PO : PageObject
    {
        private By _nameSurnameBy = By.Id("Name");
        private IWebElement _nameSurname() => _driver.FindElement(_nameSurnameBy);
        private IWebElement _userName() => _driver.FindElement(By.Id("UserName"));
        private IWebElement _deliveryAddress() => _driver.FindElement(By.Id("DeliveryAddress"));
        private IWebElement _city() => _driver.FindElement(By.Id("City"));
        private IWebElement _postalCode() => _driver.FindElement(By.Id("PostalCode"));
        private IWebElement _paymentMethod() => _driver.FindElement(By.Id("PaymentMethod"));

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        { 
        }

        public void FillInPurchaseInfo(string nameSurname, string userName, string deliveryAddress, string city, string postalCode, string paymentMethod)
        {
            // Esperamos a que el formulario esté visible
            WaitForBeingVisible(_nameSurnameBy);

            // Rellenamos los campos de texto
            _nameSurname().Clear();
            _nameSurname().SendKeys(nameSurname);

            _userName().Clear();
            _userName().SendKeys(userName);

            _deliveryAddress().Clear();
            _deliveryAddress().SendKeys(deliveryAddress);

            _city().Clear();
            _city().SendKeys(city);

            _postalCode().Clear();
            _postalCode().SendKeys(postalCode);

            // Seleccionamos el método de pago (dropdown)
            SelectElement selectElement = new SelectElement(_paymentMethod());
            selectElement.SelectByText(paymentMethod);
        }
    }
}
