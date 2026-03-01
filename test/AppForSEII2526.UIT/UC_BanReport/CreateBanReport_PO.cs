using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_BanReports
{
    public class CreateBanReport_PO : PageObject
    {
        private By _reasonBy = By.Id("Reason");
        private IWebElement _reason() => _driver.FindElement(_reasonBy);

        private By _detailedDescriptionBy = By.Id("DetailedDescription");
        private IWebElement _detailedDescription() => _driver.FindElement(_detailedDescriptionBy);

        private By _startDateBy = By.Id("StartDate");
        private IWebElement _startDate() => _driver.FindElement(_startDateBy);

        private By _endDateBy = By.Id("EndDate");
        private IWebElement _endDate() => _driver.FindElement(_endDateBy);

        private By _submitBy = By.Id("Submit");
        private IWebElement _submit() => _driver.FindElement(_submitBy);

        private By _modifyUsersBy = By.Id("ModifyUsers");
        private IWebElement _modifyUsers() => _driver.FindElement(_modifyUsersBy);

        private By _tableOfSelectedUsersBy = By.Id("TableOfSelectedUsers");
        private IWebElement _tableOfSelectedUsers() => _driver.FindElement(_tableOfSelectedUsersBy);

        public CreateBanReport_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

public void FillInBanReportInfo(string reason, string detailedDescription, DateTimeOffset startDate, DateTimeOffset endDate)
{
    WaitForBeingVisible(_reasonBy);

    _reason().Clear();
    _reason().SendKeys(reason);

    _detailedDescription().Clear();
    _detailedDescription().SendKeys(detailedDescription);

    _startDate().Clear();
    _startDate().SendKeys(startDate.ToString("yyyy-MM-dd"));
    _startDate().SendKeys(Keys.Tab);

    _endDate().Clear();
    _endDate().SendKeys(endDate.ToString("yyyy-MM-dd"));
    _endDate().SendKeys(Keys.Tab);
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

        public void FillPersonalMessageForUser(string customerId, string personalMessage)
        {
            By personalMessageBy = By.Id($"personalMessage_{customerId}");

            WaitForBeingVisible(_tableOfSelectedUsersBy);
            WaitForBeingVisible(personalMessageBy);

            IWebElement personalMessageInput = _driver.FindElement(personalMessageBy);
            personalMessageInput.Clear();
            personalMessageInput.SendKeys(personalMessage);
        }

        public bool CheckValidationError(string expectedError)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(driver => driver.PageSource.Contains(expectedError));

            return _driver.PageSource.Contains(expectedError);
        }

        public void ConfirmCreateBanReport()
        {
            By okButton = By.Id("Button_DialogOK");

            WaitForBeingClickable(okButton);
            _driver.FindElement(okButton).Click();
        }

        public void CancelCreateBanReport()
        {
            By cancelButton = By.Id("Button_DialogCancel");

            WaitForBeingClickable(cancelButton);
            _driver.FindElement(cancelButton).Click();
        }
    }
}