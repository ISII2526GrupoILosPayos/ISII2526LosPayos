using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_BanReports
{
    public class DetailBanReport_PO : PageObject
    {
        public DetailBanReport_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckBanReportDetail(string reason, string description, DateTimeOffset startDate, DateTimeOffset endDate, int usersCount)
        {
            WaitForBeingVisible(By.Id("UsersCount"));

            bool result = true;

            result = result && _driver.FindElement(By.Id("Reason")).Text.Contains(reason);
            result = result && _driver.FindElement(By.Id("Description")).Text.Contains(description);

 
            var startText = _driver.FindElement(By.Id("StartDate")).Text;
            var endText = _driver.FindElement(By.Id("EndDate")).Text;


            var actualStart = DateTimeOffset.ParseExact(startText, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            var actualEnd = DateTimeOffset.ParseExact(endText, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            result = result && (Math.Abs((actualStart - startDate).TotalMinutes) < 1);
            result = result && (Math.Abs((actualEnd - endDate).TotalMinutes) < 1);

            var actualUsersCount = int.Parse(_driver.FindElement(By.Id("UsersCount")).Text);
            result = result && (actualUsersCount == usersCount);

            return result;
        }

        public bool CheckReportedUsers(List<string[]> expectedReportedUsers)
        {
            // Tabla id="ReportedUsers" con columnas: Name, Surname, Personal message
            return CheckBodyTable(expectedReportedUsers, By.Id("ReportedUsers"));
        }
    }
}