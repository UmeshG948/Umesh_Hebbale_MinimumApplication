using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinComputerUtility;
using MinComputerUtility.AppRepositories;
using MinComputerUtility.BusinessLogic;
using MinComputerUtility.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MiniComputerTest
{
    [TestClass]
    public class BusinessLogicTest
    {
        private static BusinessDataLogic _businessLogic = new BusinessDataLogic();
        private static IRepository _repo = new AppRepository();
        private static string _filePath = ConfigurationSettings.AppSettings["FilePath"];
        private static IList<AppComputer> _csvData = _repo.GetApplicationComputerDetails(374, _filePath);

        [TestMethod]
        public void GetMinimumApplicationCopies()
        {
            var result = _businessLogic.GetMinAppCount(_csvData);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDuplicateRecordTest() 
        {
            var result = _businessLogic.GetDuplicateRecords(_csvData);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSameCustomerHavingLapAndDeskRecordTest() 
        {
            var result = _businessLogic.GetSameCustomerHavingLapAndDeskRecord(_csvData);
            Assert.IsNotNull(result);
        }
    } 
}