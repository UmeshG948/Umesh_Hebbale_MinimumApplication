using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinComputerUtility;
using MinComputerUtility.AppRepositories;
using MinComputerUtility.Model;
using System;
using System.Configuration;

namespace MiniComputerTest
{
    [TestClass]
    public class ReposistoryTest
    {
        IRepository _repsository = new AppRepository();

        string _filePath = ConfigurationSettings.AppSettings["FilePath"];

        [TestMethod]
        public void RepoSucessTest()
        {
            var result = _repsository.GetApplicationComputerDetails(374,_filePath);
            Assert.IsNotNull(result);
        }
    }

}
