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

            // Comparar el texto que se muestra en la pantalla (mismo formato que el razor)
            string expectedStartText = startDate.ToString("dd/MM/yyyy");
            string expectedEndText = endDate.ToString("dd/MM/yyyy");

            result = result && _driver.FindElement(By.Id("StartDate")).Text.Contains(expectedStartText);
            result = result && _driver.FindElement(By.Id("EndDate")).Text.Contains(expectedEndText);

            result = result && _driver.FindElement(By.Id("UsersCount")).Text.Contains(usersCount.ToString());

            return result;
        }

        public bool CheckReportedUsers(List<string[]> expectedReportedUsers)
        {
            // Tabla id="ReportedUsers" con columnas: Name, Surname, Personal message
            return CheckBodyTable(expectedReportedUsers, By.Id("ReportedUsers"));
        }
    }
}