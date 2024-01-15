using MinComputerUtility.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MinComputerUtility.BusinessLogic
{
    public class BusinessDataLogic
    {

        private readonly string[] comupterType = ConfigurationSettings.AppSettings["ComputerType"].Split(',').ToArray();

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
            //Note: Below code covers Example 3 from requirement document
            var duplicateUserIDs = appComputers
                                  .GroupBy(x => new { x.UserID, x.ComputerID })
                                  .Where(group => group.Count() > 1)
                                  .Select(group => group.Key.UserID);

            //Filter the duplicate values and get list of the information 
            var appCoumpterInfo = appComputers.Where(x => !duplicateUserIDs.Contains(x.UserID));

            foreach (var appCoumpter in appCoumpterInfo)
            {

                List<AppComputer> appComputersList = appComputers.Where(x => x.UserID == appCoumpter.UserID).ToList();

                #region User with Both desktop and laptop
                //Below logic covers Example 1 from requirement document.
                if (appComputersList.Any(x => x.ComputerType.ToUpper() == comupterType[0]) && appComputersList.Any(x => x.ComputerType.ToUpper() == comupterType[1]))
                {
                    //Below If condition is for: Already user exist then no need check with other conditions just skip
                    if (minimumReqiredApplications.Contains(appCoumpter.UserID))
                        continue;
                    //this if condition is just added to skip below additional feature
                    if (appComputersList.Count() == 2)
                    {
                        //covers Example 1 from requirement document
                        minimumReqiredApplications.Add(appCoumpter.UserID);
                    }

                    //Note: Below Additional features region is added as additional feature as per requirement these scenario are not necessary
                    //additional feature are added as part of handling worst cases these are not part of requirement
                    #region Additional features
                    else if (appComputersList.Count > 2)
                    {
                        int count = (appComputersList.Count % 2 == 0) ? appComputersList.Count / 2 : (appComputersList.Count - 1) / 2;
                        for (int i = 1; i <= count; i++)
                        {
                            minimumReqiredApplications.Add(appCoumpter.UserID);
                        }
                    }
                    #endregion Additional features
                }
                #endregion User with Both desktop and laptop

                #region Handles other criteria 
                //Below code covers Example 2 where same user ID having same application installed on (2 desktop or 2 laptop) or only (1 desktop or 1 laptop)
                else
                {
                    minimumReqiredApplications.Add(appComputersList.FirstOrDefault(x => x.UserID == appCoumpter.UserID).UserID);
                }
                #endregion Handles other criteria 
            }
            minimumReqiredApplications.AddRange(duplicateUserIDs);
            return minimumReqiredApplications.Count();
        }
        #endregion Minimum Application required


        #region Methods are for test purpose only
        public IEnumerable<int> GetDuplicateRecords(IList<AppComputer> appComputerList)
        {
            return appComputerList.GroupBy(x => new { x.UserID, x.ComputerID })
                                  .Where(group => group.Count() > 1)
                                  .Select(group => group.Key.UserID);
        }

        public IEnumerable<int> GetSameCustomerHavingLapAndDeskRecord(IList<AppComputer> appComputers)
        {
            List<int> count = new List<int>();
            var duplicateUserIDs = GetDuplicateRecords(appComputers);

            var appCoumpterInfo = appComputers.Where(x => !duplicateUserIDs.Contains(x.UserID));

            foreach (var appCoumpter in appCoumpterInfo)
            {

                List<AppComputer> appComputersList = appComputers.Where(x => x.UserID == appCoumpter.UserID).ToList();

                if (appComputersList.Any(x => x.ComputerType.ToUpper() == comupterType[0]) && appComputersList.Any(x => x.ComputerType.ToUpper() == comupterType[1]))
                {
                    if (count.Contains(appCoumpter.UserID))
                        continue;

                    if (appComputersList.Count() == 2)
                    {
                        count.Add(appCoumpter.UserID);
                    }
                    else if (appComputersList.Count > 2)
                    {
                        int num = (appComputersList.Count % 2 == 0) ? appComputersList.Count / 2 : (appComputersList.Count - 1) / 2;
                        for (int i = 1; i <= num; i++)
                        {
                            count.Add(appCoumpter.UserID);
                        }
                    }
                }
            }
            return count;
        }

        public IEnumerable<int> GetRecordsWithMultipleOrSingleComputerType(IList<AppComputer> appComputers, List<int> userToExclude)
        {

            List<int> count = new List<int>();
           var appCoumpterInfo = appComputers.Where(x => !userToExclude.Contains(x.UserID));

            foreach (var appCoumpter in appCoumpterInfo)
            {
                count.Add(appCoumpter.UserID);
            }
            return count;
        }

        #endregion Methods are for test purpose only
    }
}
