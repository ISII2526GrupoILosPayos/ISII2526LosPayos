using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UC_PurchaseProducts_UIT : UC_UIT
    {
        private SelectProductsForPurchase_PO selectProductsForPurchase_PO;

        public UC_PurchaseProducts_UIT(ITestOutputHelper output) : base(output)
        {
        }

        private void Precondition_perform_login()
        {
            Perform_login("Luis.melero1@alu.uclm.es", "Password1234%");
        }

        private void InitialStepsForPurchaseProducts()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectProductsForPurchase_PO.WaitForBeingVisible(By.Id("CreatePurchaseOrder"));
            //we click on the menu
            _driver.FindElement(By.Id("CreatePurchaseOrder")).Click();
        }
    }
}
