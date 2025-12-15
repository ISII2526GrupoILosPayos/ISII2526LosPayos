using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

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

        private By _dialogBy = By.Id("DialogOKSaveDelete");
        private By _dialogOkBy = By.Id("Button_DialogOK");
        private By _dialogCancelBy = By.Id("Button_DialogCancel");

        private By _tableSelectedUsersBy = By.Id("TableOfSelectedUsers");

        private IWebElement _reason() => _driver.FindElement(_reasonBy);
        private IWebElement _detailedDescription() => _driver.FindElement(_detailedDescriptionBy);
        private IWebElement _submit() => _driver.FindElement(_submitBy);
        private IWebElement _modifyUsers() => _driver.FindElement(_modifyUsersBy);

        public CreateBanReport_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FillInBanReportInfo(string reason, string detailedDescription)
        {
            WaitForBeingVisible(_reasonBy);

            _reason().Clear();
            _reason().SendKeys(reason ?? "");

            _detailedDescription().Clear();
            _detailedDescription().SendKeys(detailedDescription ?? "");
        }

        public void FillInBanReportDates(DateTimeOffset start, DateTimeOffset end)
        {
            WaitForBeingVisible(_startDateBy);
            InputDateInDatePicker(_startDateBy, start.DateTime);

            WaitForBeingVisible(_endDateBy);
            InputDateInDatePicker(_endDateBy, end.DateTime);
        }

        //public void FillInBanReportDates(DateTime start, DateTime end)
        //{
        //    WaitForBeingVisible(_startDateBy);
        //    InputDateInDatePicker(_startDateBy, start);

        //    WaitForBeingVisible(_endDateBy);
        //    InputDateInDatePicker(_endDateBy, end);
        //}

        public void FillPersonalMessage(string customerId, string message)
        {
            var by = By.Id($"personalMessage_{customerId}");
            WaitForBeingVisible(by);

            var input = _driver.FindElement(by);
            input.Clear();
            input.SendKeys(message ?? "");
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

        public void ConfirmDialog()
        {
            WaitForBeingVisible(_dialogBy);
            WaitForBeingClickable(_dialogOkBy);
            _driver.FindElement(_dialogOkBy).Click();
        }

        public void CancelDialog()
        {
            WaitForBeingVisible(_dialogBy);
            WaitForBeingClickable(_dialogCancelBy);
            _driver.FindElement(_dialogCancelBy).Click();
        }

        public bool CheckSelectedUsersTable(List<string[]> expectedRows)
        {
            WaitForBeingVisible(_tableSelectedUsersBy);
            return CheckBodyTable(expectedRows, _tableSelectedUsersBy);
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }

        public bool CheckModalError(string expectedError)
        {
            return CheckModalBodyText(expectedError, _dialogBy);
        }
    }
}
