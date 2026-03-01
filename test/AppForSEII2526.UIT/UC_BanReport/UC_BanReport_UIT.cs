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
        public void UC77_AF2_4_ButtonNotavailable()
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
    }
}