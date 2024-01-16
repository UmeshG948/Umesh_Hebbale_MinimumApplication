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
        private static IList<AppComputer> _csvData = _repo.GetApplicationComputerDetails(169, _filePath);

        [TestMethod]
        public void GetMinimumApplicationCopies()
        {            

            var result = _businessLogic.GetMinAppCount(_csvData);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDuplicateRecordTest() 
        {
            List<long> duplicateRecords;
            _businessLogic.GetDuplicateRecords(_csvData, out duplicateRecords);
            Assert.IsNotNull(duplicateRecords);
        }

        [TestMethod]
        public void GetSameCustomerHavingLapAndDeskRecordTest() 
        {
            List<long> count;
            _businessLogic.GetSameCustomerHavingLapAndDeskRecord(_csvData, out count);
            Assert.IsNotNull(count);
        }

        [TestMethod]
        public void GetRecordsWithMultipleOrSingleComputerTypeTest()
        {
            List<long> count;
            List<long> duplicateRecords;
            List<long> countedCustomerId;
            _businessLogic.GetDuplicateRecords(_csvData, out duplicateRecords);
            _businessLogic.GetSameCustomerHavingLapAndDeskRecord(_csvData, out count);
            count.AddRange(duplicateRecords);
            _businessLogic.GetRecordsWithMultipleOrSingleComputerType(_csvData, count, out countedCustomerId);
            Assert.IsNotNull(countedCustomerId);
        }
    } 
}