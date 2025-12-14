using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_BanReport
{
    public class CreateBanReport_PO : PageObject
    {
        private By _reasonBy = By.Id("Reason");
        private By _detailedDescriptionBy = By.Id("DetailedDescription");
        private By _startDateBy = By.Id("StartDate");
        private By _endDateBy = By.Id("EndDate");

        private By _submitBy = By.Id("Submit");
        private By _modifyUsersBy = By.Id("ModifyUsers");

        private By _tableSelectedUsersBy = By.Id("TableOfSelectedUsers");

        private By _modalBy = By.Id("DialogOKSaveDelete");
        private By _modalOkBy = By.Id("Button_DialogOK");
        private By _modalCancelBy = By.Id("Button_DialogCancel");

        private IWebElement _reason() => _driver.FindElement(_reasonBy);
        private IWebElement _detailedDescription() => _driver.FindElement(_detailedDescriptionBy);
        private IWebElement _submit() => _driver.FindElement(_submitBy);
        private IWebElement _modifyUsers() => _driver.FindElement(_modifyUsersBy);

        public CreateBanReport_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FillInReportInfo(string reason, string detailedDescription, DateTimeOffset start, DateTimeOffset end)
        {
            WaitForBeingVisible(_reasonBy);

            _reason().Clear();
            _reason().SendKeys(reason);

            _detailedDescription().Clear();
            _detailedDescription().SendKeys(detailedDescription);

            InputDateInDatePicker(_startDateBy, start.DateTime);
            InputDateInDatePicker(_endDateBy, end.DateTime);
        }

        public void FillPersonalMessage(string customerId, string personalMessage)
        {
            var by = By.Id($"personalMessage_{customerId}");
            WaitForBeingVisible(by);

            var input = _driver.FindElement(by);
            input.Clear();
            input.SendKeys(personalMessage);
        }

        public void PressCreateBanReport()
        {
            WaitForBeingClickable(_submitBy);
            _submit().Click();
        }

        public void PressModifyUsers()
        {
            WaitForBeingClickable(_modifyUsersBy);
            _modifyUsers().Click();
        }

        public void ConfirmCreateReport(bool ok)
        {
            WaitForBeingVisible(_modalBy);

            if (ok)
                _driver.FindElement(_modalOkBy).Click();
            else
                _driver.FindElement(_modalCancelBy).Click();
        }

        public bool CheckSelectedUsersTable(List<string[]> expectedRows)
        {
            return CheckBodyTable(expectedRows, _tableSelectedUsersBy);
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }

        public bool CheckModalError(string expectedError)
        {
            return CheckModalBodyText(expectedError, _modalBy);
        }
    }
}
