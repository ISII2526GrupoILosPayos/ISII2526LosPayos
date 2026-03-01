using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using Xunit;
using Xunit.Abstractions;
using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_BanReport;
using AppForSEII2526.UIT.UC_Purchase;
using System.Threading;

namespace AppForSEII2526.UIT.UC_BanReports
{
    public class UC_BanUser_UIT : UC_UIT
    {
        private SelectUsersForBan_PO selectUsersForBan_PO;

        private const string userName1 = "Kylian";
        private const string userSurname1 = "Mbappe";
        private const string creationDate1 = "10/10/2025";
        private const string unproComplaint1 = "Queja (2)";

        private const string userName2 = "Pau";
        private const string userSurname2 = "Femenia";
        private const string creationDate2 = "12/11/2004";
        private const string unproComplaint2 = "Denuncia (1)";

        private const string reason = "hola hola hola";
        private const string detailedDescription = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private const string startdate = "01/03/2026";
        private const string enddate = "04/03/2026";

        public UC_BanUser_UIT(ITestOutputHelper output) : base(output)
        {
            selectUsersForBan_PO = new SelectUsersForBan_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("Prueba1-@gmail.com", "Prueba1-");
        }


        private void InitialSteps_SelectUsersForBan()
        {
            Precondition_perform_login();
            selectUsersForBan_PO.WaitForBeingClickable(By.Id("ReportUserOrder"));
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("ReportUserOrder")).Click();

        }

        [Theory]
        [InlineData(userName1, userSurname1, creationDate1, unproComplaint1, "Mbappe", "")]
        [InlineData(userName2, userSurname2, creationDate2, unproComplaint2, "", "Denuncia")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC34_AF1_2_3filtering(string name, string surname, string date, string unproComplaint, string filterSurname, string type)
        {
            //Arrange
            InitialSteps_SelectUsersForBan();
            var expectedProducts = new List<string[]> { new string[] {  name,  surname,  date, unproComplaint }, };

            //Act
            selectUsersForBan_PO.searchUser(filterSurname, type);

            //Assert
            Assert.True(selectUsersForBan_PO.CheckListOfUsers(expectedProducts));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC34_AF2_4_ButtonNotavailable()
        {
            //Arrange
            InitialSteps_SelectUsersForBan();

            //Act
            selectUsersForBan_PO.searchUser("", "");
            selectUsersForBan_PO.AddUserToBanReport("1");
            selectUsersForBan_PO.RemoveUserFromBanReport("1");

            //Assert

            Assert.True(selectUsersForBan_PO.ContinueNotAvailable());

        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC34_AF23_5_ButtonAvailable()
        {
            //Arrange
            InitialSteps_SelectUsersForBan();

            //Act
            selectUsersForBan_PO.searchUser("", "");
            selectUsersForBan_PO.AddUserToBanReport("1");
            selectUsersForBan_PO.AddUserToBanReport("2");
            selectUsersForBan_PO.RemoveUserFromBanReport("1");

            //Assert

            Assert.True(selectUsersForBan_PO.ContinueAvailable());

        }


        [Theory]
        [InlineData(reason,"", startdate, enddate, "The DetailedDescription field is required.")]
        [InlineData("",detailedDescription, startdate, enddate, "The Reason field is required.")]
        [InlineData(reason, detailedDescription, "adios", enddate, "The StartDate field must be a date.")]
        [InlineData(reason, detailedDescription, startdate, "adios", "The EndDate field must be a date.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC34_AF5_6_MissingInformation(string reason, string description, string startdate, string enddate, string motivoError)
        {
            var createBanReport_PO = new CreateBanReport_PO(_driver, _output);

            //Arrange
            InitialSteps_SelectUsersForBan();

            //Act
            selectUsersForBan_PO.searchUser("", "");
            selectUsersForBan_PO.AddUserToBanReport("2");
            selectUsersForBan_PO.Continue();

            createBanReport_PO.FillInBanReportInfo(reason, description, startdate, enddate);
            createBanReport_PO.PressCreateBanReport();
            //Assert

            Assert.True(createBanReport_PO.CheckValidationError(motivoError));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC34_BF_1()
        {
            var createBanReport_PO = new CreateBanReport_PO(_driver, _output);
            var detailsBanReport_PO = new DetailBanReport_PO(_driver, _output);

            var startDate = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);
            var endDate = new DateTimeOffset(2026, 3, 4, 0, 0, 0, TimeSpan.Zero);

            //Arrange
            InitialSteps_SelectUsersForBan();

            //Act
            selectUsersForBan_PO.searchUser("", "");
            selectUsersForBan_PO.AddUserToBanReport("1");
            selectUsersForBan_PO.Continue();

            createBanReport_PO.FillInBanReportInfo("hola hola hola", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "01/03/2026", "04/03/2026");
            createBanReport_PO.PressCreateBanReport();
            createBanReport_PO.ConfirmCreateBanReport();


            //Assert

            Assert.True(detailsBanReport_PO.CheckBanReportDetail("hola hola hola", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", startDate, endDate,1));

        }
    }
}