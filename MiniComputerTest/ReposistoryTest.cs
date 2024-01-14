using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinComputerUtility;
using MinComputerUtility.AppRepositories;
using MinComputerUtility.Model;
using System;

namespace MiniComputerTest
{
    [TestClass]
    public class ReposistoryTest
    {
        IRepository _repsository = new AppRepository();
        
        [TestMethod]
        public void RepoSucessTest()
        {
            var result = _repsository.GetApplicationComputerDetails(374);
            Assert.IsNotNull(result);
        }
    }

}
