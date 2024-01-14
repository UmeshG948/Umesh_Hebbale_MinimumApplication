using MinComputerUtility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinComputerUtility.BusinessLogic
{
    public class BusinessDataLogic
    {

        private readonly string[] comupterType = new string[]
                                                 {
                                                    "LAPTOP",
                                                    "DESKTOP"
                                                 };
       


        /// <summary>
        /// Performs the logical operation and gets the minimum number of application reqired per user.
        /// </summary>
        /// <param name="appComputers">Pass appliaction computer details</param>
        /// <returns>returns minimum application required to the users available for that application</returns>
       #region Minimum Application required
        public int GetMinAppCount(IList<AppComputer> appComputers)
        {

            List<int> minimumReqiredApplications = new List<int>();

            //Below logic gets all the USerID having duplicate values.
            var dulpicateRecords = appComputers.GroupBy(x => new { x.UserID, x.ComputerID })
                                  .Where(y => y.Select(x => x.UserID).Count() > 1 && 
                                  y.Select(x => x.ComputerID).Count() > 1).Select(x => x.Select(y => y.UserID)
                                  .FirstOrDefault()).ToList();

            //Filter the duplicate values and get list of the information 
            var appCoumpterInfo = appComputers.Where(x => !dulpicateRecords.Contains(x.UserID))
                                  .GroupBy(x => x.UserID).SelectMany(x => x).ToList();

            foreach (var appCoumpter in appCoumpterInfo)
            {
                //Below If condition is for: Already user exist then no need check with other conditions just skip
                if (minimumReqiredApplications.Contains(appCoumpter.UserID))
                {
                    continue;
                }

                List<AppComputer> appComputersList = appComputers.Where(x => x.UserID == appCoumpter.UserID).ToList();

                #region User with Both desktop and laptop
                if (appComputersList.Select(x => x.ComputerType.ToUpper()).Contains(comupterType[0]) && appComputersList.Select(x => x.ComputerType.ToUpper()).Contains(comupterType[1]))
                {
                    //this if condition is just added to skip below additional feature
                    if (appComputersList.Count() == 2)
                    {
                        minimumReqiredApplications.Add(appCoumpter.UserID);
                    }

                    //Below  are the Scenario is added as additional feature as per requirement these scenario in not require
                    #region Additional features
                    if (appComputersList.Count() % 2 == 0)
                    {

                        for (int i = 1; i <= appComputersList.Count() / 2; i++)
                        {
                            minimumReqiredApplications.Add(appCoumpter.UserID);
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= (appComputersList.Count() - 1) / 2; i++)
                        {
                            minimumReqiredApplications.Add(appCoumpter.UserID);
                        }
                    }
                    #endregion Additional features
                }
                #endregion User with Both desktop and laptop

                //if ((appComputersList.All(x => x.ComputerType.ToUpper().Equals(comupterType[0]))) || (appComputersList.All(x => x.ComputerType.ToUpper().Equals(comupterType[1]))))
                #region Handles other criteria 
                else
                {
                    minimumReqiredApplications.AddRange(appComputersList.Select(x => x.UserID));
                }
                #endregion Handles other criteria 
            }
            minimumReqiredApplications.AddRange(dulpicateRecords);
            return minimumReqiredApplications.Count();
        }
        #endregion Minimum Application required
    }
}
