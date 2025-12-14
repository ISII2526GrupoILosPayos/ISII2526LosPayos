using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_BanReport
{
    public class DetailBanReport_PO : PageObject
    {
        public DetailBanReport_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        private string TextOfFirstExistingId(params string[] ids)
        {
            foreach (var id in ids)
            {
                var elements = _driver.FindElements(By.Id(id));
                if (elements != null && elements.Count > 0)
                    return elements[0].Text;
            }

            _output.WriteLine($"No element found with any of these ids: {string.Join(", ", ids)}");
            return "";
        }

        private By FirstExistingBy(params string[] ids)
        {
            foreach (var id in ids)
            {
                var elements = _driver.FindElements(By.Id(id));
                if (elements != null && elements.Count > 0)
                    return By.Id(id);
            }

            return By.Id(ids[0]);
        }

        public bool CheckBanReportDetail(
            string reason,
            string description,
            DateTime startDate,
            DateTime endDate)
        {
            WaitForBeingVisible(By.Id("Reason"));

            bool result = true;

            result = result && TextOfFirstExistingId("Reason").Contains(reason);

            var actualDesc = TextOfFirstExistingId("Description", "DetailedDescription");
            result = result && actualDesc.Contains(description);

            string startExpected = startDate.ToString("dd/MM/yyyy");
            string endExpected = endDate.ToString("dd/MM/yyyy");

            var actualStart = TextOfFirstExistingId("StartDate", "Start");
            var actualEnd = TextOfFirstExistingId("EndDate", "End");

            result = result && actualStart.Contains(startExpected);
            result = result && actualEnd.Contains(endExpected);

            return result;
        }

        public bool CheckListOfReportedUsers(List<string[]> expectedUsers)
        {
            var tableBy = FirstExistingBy("TableOfReportedUsers", "ReportedUsers", "TableOfUsers");

            return CheckBodyTable(expectedUsers, tableBy);
        }
    }
}
