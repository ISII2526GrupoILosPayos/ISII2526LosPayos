using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace AppForSEII2526.UIT.UC_BanReport
{
    public class SelectUsersForBan_PO : PageObject
    {
        private readonly By _surnameBy = By.Id("surnameFilter");
        private readonly By _complaintTypeBy = By.Id("complaintTypeFilter");
        private readonly By _creationDateBy = By.Id("creationDateFilter");

        private readonly By _showBanCartBy = By.Id("showBanCart");
        private readonly By _searchUsersBy = By.Id("searchUsers");
        private readonly By _continueBy = By.Id("Continue");

        private readonly By _tableOfUsersBy = By.Id("TableOfUsersForBan");
        private readonly By _tableOfSelectedUsersBy = By.Id("TableOfSelectedUsers");

        private IWebElement Surname() => _driver.FindElement(_surnameBy);
        private IWebElement ComplaintType() => _driver.FindElement(_complaintTypeBy);
        private IWebElement CreationDate() => _driver.FindElement(_creationDateBy);

        private IWebElement ShowBanCartButton() => _driver.FindElement(_showBanCartBy);
        private IWebElement SearchUsersButton() => _driver.FindElement(_searchUsersBy);
        private IWebElement ContinueButton() => _driver.FindElement(_continueBy);

        public SelectUsersForBan_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FilterUsers(string? surnameFilter, string? complaintTypeFilter, DateTime? creationDate)
        {
            WaitForBeingVisible(_surnameBy);

            Surname().Clear();
            if (!string.IsNullOrWhiteSpace(surnameFilter))
                Surname().SendKeys(surnameFilter);

            ComplaintType().Clear();
            if (!string.IsNullOrWhiteSpace(complaintTypeFilter))
                ComplaintType().SendKeys(complaintTypeFilter);

            CreationDate().Clear();
            if (creationDate.HasValue)
            {
                InputDateInDatePicker(_creationDateBy, creationDate.Value);
            }

            SearchUsersButton().Click();

            Thread.Sleep(2000);
        }

        public void SelectUsersByIds(List<string> userIds)
        {
            foreach (var id in userIds)
            {
                var addBtn = By.Id($"userToBan_{id}");
                WaitForBeingVisible(addBtn);
                _driver.FindElement(addBtn).Click();
            }
        }

        public void SelectFirstUsers(int numberOfUsers)
        {
            WaitForBeingVisible(_tableOfUsersBy);

            var table = _driver.FindElement(_tableOfUsersBy);

            var addButtons = table.FindElements(By.CssSelector("tbody button[id^='userToBan_']")).ToList();

            for (int i = 0; i < numberOfUsers && i < addButtons.Count; i++)
                addButtons[i].Click();
        }

        public void ToggleBanCart()
        {
            WaitForBeingClickable(_showBanCartBy);
            ShowBanCartButton().Click();
        }

        public void ModifyBanCart_RemoveUser(string customerId)
        {
            ToggleBanCart();

            var removeBtn = By.Id($"removeUser_{customerId}");
            WaitForBeingVisible(removeBtn);
            _driver.FindElement(removeBtn).Click();
        }

        public void ContinueToCreateBanReport()
        {
            WaitForBeingClickable(_continueBy);
            ContinueButton().Click();
        }

        public bool CheckContinueDisabled()
        {
            return !ContinueButton().Enabled;
        }

        public int GetUsersCountInTable()
        {
            if (!_driver.FindElements(_tableOfUsersBy).Any())
                return 0;

            var table = _driver.FindElement(_tableOfUsersBy);
            return table.FindElements(By.CssSelector("tbody tr")).Count;
        }

        public bool CheckSelectedUsersCount(int expected)
        {
            ToggleBanCart();

            if (!_driver.FindElements(_tableOfSelectedUsersBy).Any())
                return expected == 0;

            var table = _driver.FindElement(_tableOfSelectedUsersBy);
            var rows = table.FindElements(By.CssSelector("tbody tr"));
            return rows.Count == expected;
        }

        public bool CheckUsersTableContainsRow(string userId)
        {
            var row = By.Id($"UserData_{userId}");
            return _driver.FindElements(row).Any();
        }

        public bool CheckSelectedUserRowExists(string customerId)
        {
            var row = By.Id($"SelectedUser_{customerId}");
            return _driver.FindElements(row).Any();
        }
    }
}
