namespace AppForSEII2526.UIT.UC_BanReport
{
    public class SelectUsersForBan_PO : PageObject
    {
        By buttonSearchUsers = By.Id("searchUsers");
        By buttonContinue = By.Id("Continue");
        By surnameFilter = By.Id("surnameFilter");
        By complaintFilter = By.Id("complaintTypeFilter");
        By creationDate = By.Id("creationDateFilter");
        By tableUsers = By.Id("TableOfUsersForBan");
        By showSelectedUsers = By.Id("showBanCart");
        By tableSelectedUsers = By.Id("TableOfSelectedUsers");



        IWebElement _continueButton() => _driver.FindElement(buttonContinue);

        public SelectUsersForBan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        public void searchUser(string surname, string type)
        {
            WaitForBeingClickable(surnameFilter);
            _driver.FindElement(surnameFilter).Clear();
            _driver.FindElement(surnameFilter).SendKeys(surname);

            _driver.FindElement(complaintFilter).Clear();
            _driver.FindElement(complaintFilter).SendKeys(surname);

            _driver.FindElement(buttonSearchUsers).Click();
        }

        public bool CheckListOfUsers(List<string[]> expectedUsers)
        {

            return CheckBodyTable(expectedUsers, tableUsers);
        }

        public void AddUserToBanReport(string userNumber)
        {
            WaitForBeingClickable(By.Id("userToBan_" + userNumber));

            _driver.FindElement(By.Id("userToBan_" + userNumber)).Click();
        }

        public void RemoveUserFromBanReport(string userNumber)
        {

            WaitForBeingClickable(showSelectedUsers);
            _driver.FindElement(showSelectedUsers).Click();
            _driver.FindElement(By.Id("removeUser_" + userNumber)).Click();
        }

        public bool ContinueNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonContinue).Displayed == false;
        }

        public bool ContinueAvailable()
        {
            return _driver.FindElement(buttonContinue).Displayed;
        }

        public void Continue()
        {
            WaitForBeingClickable(buttonContinue);
            _continueButton().Click();
        }
    }
}
