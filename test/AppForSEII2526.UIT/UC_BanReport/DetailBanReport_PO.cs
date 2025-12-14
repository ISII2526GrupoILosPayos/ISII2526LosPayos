using System;
using System.Collections.Generic;
using System.Globalization;

namespace AppForSEII2526.UIT.UC_BanReport
{
    public class DetailBanReport_PO : PageObject
    {
        public DetailBanReport_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public bool CheckBanReportDetail(
            string reason,
            string detailedDescription,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {

            WaitForBeingVisible(By.Id("Reason"));

            bool result = true;

            result = result && _driver.FindElement(By.Id("Reason")).Text.Contains(reason);
            result = result && _driver.FindElement(By.Id("DetailedDescription")).Text.Contains(detailedDescription);

            string startExpected = startDate.DateTime.ToString("dd/MM/yyyy");
            string endExpected = endDate.DateTime.ToString("dd/MM/yyyy");

            result = result && _driver.FindElement(By.Id("StartDate")).Text.Contains(startExpected);
            result = result && _driver.FindElement(By.Id("EndDate")).Text.Contains(endExpected);

            return result;
        }

        public bool CheckListOfReportedUsers(List<string[]> expectedUsers)
        {
            return CheckBodyTable(expectedUsers, By.Id("TableOfReportedUsers"));
        }
    }
}
