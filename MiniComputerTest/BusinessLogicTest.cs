using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinComputerUtility;
using MinComputerUtility.AppRepositories;
using MinComputerUtility.BusinessLogic;
using MinComputerUtility.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniComputerTest
{
    [TestClass]
    public class BusinessLogicTest
    {
        BusinessDataLogic _businessLogic = new BusinessDataLogic();
        IRepository _repo = new AppRepository();
        [TestMethod]
        public void GetMinimumApplicationCopies()
        {
            var result = _businessLogic.GetMinAppCount(_repo.GetApplicationComputerDetails(374));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetOnlyDuplicateRecords()
        {
            var appCustomerData = _repo.GetApplicationComputerDetails(374);

            var duplicateRecords = appCustomerData.GroupBy(x => new { x.UserID, x.ComputerID })
                                  .Where(y => y.Select(x => x.UserID).Count() > 1 &&
                                  y.Select(x => x.ComputerID).Count() > 1).Select(x => x.Select(y => y.UserID)
                                  .FirstOrDefault()).ToList();

            Assert.IsNotNull(duplicateRecords);
        }

        [TestMethod]
        public void CheckSingleInstanceApplication()
        {
            int count = 0;
            var appCustomerData = _repo.GetApplicationComputerDetails(374);

            var dulpicateRecords = appCustomerData.GroupBy(x => new { x.UserID, x.ComputerID })
                                  .Where(y => y.Select(x => x.UserID).Count() > 1 &&
                                  y.Select(x => x.ComputerID).Count() > 1).Select(x => x.Select(y => y.UserID)
                                  .FirstOrDefault()).ToList();

            var appCoumpterInfo = appCustomerData.Where(x => !dulpicateRecords.Contains(x.UserID))
                                  .GroupBy(x => x.UserID).SelectMany(x => x).ToList();

            foreach (var appCoumpter in appCoumpterInfo)
            {

                List<AppComputer> appComputersList = appCustomerData.Where(x => x.UserID == appCoumpter.UserID).ToList();

                if (appComputersList.Select(x => x.ComputerType.ToUpper()).Contains("DESKTOP") && appComputersList.Select(x => x.ComputerType.ToUpper()).Contains("LAPTOP"))
                {

                    if (appComputersList.Count() == 2)
                    {
                        count++;
                    }

                    if (appComputersList.Count() % 2 == 0)
                    {

                        for (int i = 1; i <= appComputersList.Count() / 2; i++)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= (appComputersList.Count() - 1) / 2; i++)
                        {
                            count++;
                        }
                    }
                }
            }

            Assert.AreEqual(6, count);
        }

        [TestMethod]
        public void TestApplicationWithSameComputerTypeWithoutDuplicate()
        {
            int count = 0;
            var appCustomerData = _repo.GetApplicationComputerDetails(374);

            var dulpicateRecords = appCustomerData.GroupBy(x => new { x.UserID, x.ComputerID })
                      .Where(y => y.Select(x => x.UserID).Count() > 1 &&
                      y.Select(x => x.ComputerID).Count() > 1).Select(x => x.Select(y => y.UserID)
                      .FirstOrDefault()).ToList();

            var appCoumpterInfo = appCustomerData.Where(x => !dulpicateRecords.Contains(x.UserID))
                                  .GroupBy(x => x.UserID).SelectMany(x => x).ToList();


            foreach (var appCoumpter in appCoumpterInfo)
            {
                List<AppComputer> appComputersList = appCustomerData.Where(x => x.UserID == appCoumpter.UserID).ToList();

                if(appComputersList.Select(x => x.ComputerType.ToUpper()).Contains("DESKTOP") && appComputersList.Select(x => x.ComputerType.ToUpper()).Contains("LAPTOP"))
                {
                    continue;
                }

                else
                {
                    count++;
                }
            }

            Assert.AreEqual(177, count);
        }
    } 
}