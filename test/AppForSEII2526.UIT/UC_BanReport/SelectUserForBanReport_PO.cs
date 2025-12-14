using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_BanReport
{
    public class SelectUsersForBan_PO : PageObject
    {
        private By _surnameBy = By.Id("surnameFilter");
        private By _complaintTypeBy = By.Id("complaintTypeFilter");
        private By _creationDateBy = By.Id("creationDateFilter");

        private By _showBanCartBy = By.Id("showBanCart");
        private By _searchUsersBy = By.Id("searchUsers");
        private By _continueBy = By.Id("Continue");

        private By _tableOfUsersBy = By.Id("TableOfUsersForBan");
        private By _tableOfSelectedUsersBy = By.Id("TableOfSelectedUsers");

        private IWebElement _surname() => _driver.FindElement(_surnameBy);
        private IWebElement _complaintType() => _driver.FindElement(_complaintTypeBy);
        private IWebElement _creationDate() => _driver.FindElement(_creationDateBy);

        private IWebElement _showBanCartButton() => _driver.FindElement(_showBanCartBy);
        private IWebElement _searchUsersButton() => _driver.FindElement(_searchUsersBy);
        private IWebElement _continueButton() => _driver.FindElement(_continueBy);

        public SelectUsersForBan_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FilterUsers(string surnameFilter, string complaintTypeFilter, DateTime? creationDate)
        {
            WaitForBeingVisible(_surnameBy);

            _surname().Clear();
            _surname().SendKeys(surnameFilter ?? "");

            _complaintType().Clear();
            _complaintType().SendKeys(complaintTypeFilter ?? "");

            if (creationDate != null)
            {
                InputDateInDatePicker(_creationDateBy, creationDate.Value);
            }

            _searchUsersButton().Click();

            System.Threading.Thread.Sleep(2000);
        }

        public void SelectUsersByIds(List<string> userIds)
        {
            foreach (var id in userIds)
            {
                var addButton = By.Id($"userToBan_{id}");
                WaitForBeingVisible(addButton);
                _driver.FindElement(addButton).Click();
            }
        }

        public void SelectFirstUsers(int numberOfUsers)
        {
            WaitForBeingVisible(_tableOfUsersBy);

            var table = _driver.FindElement(_tableOfUsersBy);
            var addButtons = table.FindElements(By.CssSelector("tbody button[id^='userToBan_']"));

            for (int i = 0; i < numberOfUsers && i < addButtons.Count; i++)
                addButtons[i].Click();
        }

        public void ContinueToCreateBanReport()
        {
            WaitForBeingClickable(_continueBy);
            _continueButton().Click();
        }

        public void ModifyBanCart_RemoveUser(string customerId)
        {
            _showBanCartButton().Click();
            var removeBtn = By.Id($"removeUser_{customerId}");
            WaitForBeingVisible(removeBtn);
            _driver.FindElement(removeBtn).Click();
        }

        public bool CheckContinueDisabled()
        {
            return !(_continueButton().Enabled);
        }

        public bool CheckSelectedUsersCount(int expected)
        {
            _showBanCartButton().Click();

            var tableExists = _driver.FindElements(_tableOfSelectedUsersBy).Any();
            if (!tableExists)
                return expected == 0;

            var table = _driver.FindElement(_tableOfSelectedUsersBy);
            var rows = table.FindElements(By.CssSelector("tbody tr"));
            return rows.Count == expected;
        }

        public int GetUsersCountInTable()
        {
            var tableExists = _driver.FindElements(_tableOfUsersBy).Any();
            if (!tableExists)
                return 0;

            var table = _driver.FindElement(_tableOfUsersBy);
            return table.FindElements(By.CssSelector("tbody tr")).Count;
        }

    }
}
