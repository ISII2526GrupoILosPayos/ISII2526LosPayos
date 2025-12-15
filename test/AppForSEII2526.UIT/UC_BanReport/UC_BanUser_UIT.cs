using AppForSEII2526.UIT.UC_Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_BanReport
{
    public class UC_BanUser_UIT : UC_UIT
    {

        private SelectUsersForBan_PO selectUserForBan_PO;
        private const string surname1 = "Feme";
        private const string type1 = "Mal";

        private void Precondition_perform_login()
        {
            Perform_login("Hugo.hernandez2@alu.uclm.es", "Password1234%");
        }

        private void InitialStepsForBanReport()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectUserForBan_PO.WaitForBeingVisible(By.Id("CreateBanReport"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateBanReport")).Click();


        }

        [Fact]
        //Theory
        //Aqui falta el inline data
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_BF_AF1_AF1_ExamTesting()
        {

            var createBanReport_PO = new CreateBanReport_PO(_driver, _output);
            var detailBanReport_PO = new DetailBanReport_PO(_driver, _output);

            //Arrange
            InitialStepsForBanReport();
            //var expectedProducts = 

            //Act
            selectUserForBan_PO.FilterUsers("feme", null, null);
             
            selectUserForBan_PO.SelectFirstUsers(1);
            selectUserForBan_PO.FilterUsers(null, "mal", null);
            selectUserForBan_PO.SelectFirstUsers(1);
            selectUserForBan_PO.RemoveUserFromBanReport("1");
            selectUserForBan_PO.ContinueToCreateBanReport();


            createBanReport_PO.PressCreateBanReport();


            //Assert
            Assert.True(detailBanReport_PO.CheckBanReportDetail());




        }
    }
}
