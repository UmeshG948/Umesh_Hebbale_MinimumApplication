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
            IDictionary<long, int> keyValuePairs = new Dictionary<long, int>();
            List<AppComputer> appComputersList = new List<AppComputer>();

            //Below logic gets all the USerID having duplicate values.
            //Note: Below code covers Example 3 from requirement document
            var duplicateUserIDs = appComputers
                                  .GroupBy(x => new { x.UserID, x.ComputerID })
                                  .Where(group => group.Count() > 1)
                                  .Select(group => new { group.Key.ComputerID, group.Key.UserID });

            //Filter the duplicate values and get list of the information 
            var appCoumpterInfo = appComputers.Where(x => !duplicateUserIDs.Any(y => y.ComputerID == x.ComputerID));

            foreach (var appCoumpter in appCoumpterInfo)
            {

                appComputersList = appCoumpterInfo
                                   .Where(x => x.UserID == appCoumpter.UserID && !keyValuePairs.ContainsKey(x.ComputerID))
                                   .ToList();


                //Below logic covers Example 1 from requirement document.
                if ((appComputersList.Any(x => x.ComputerType.ToUpper() == comupterType[0]) && appComputersList.Any(x => x.ComputerType.ToUpper() == comupterType[1])))
                {
                    //Below query is used to avoid multipe mapping to on computer type
                    //Ex: same userID having 2 desktop and 2 laptop then it should count only 2 for that user.
                    AppComputer appComputerWithDiffCompType = appCoumpter.ComputerType.Trim().ToUpper() == comupterType[0] ?
                                              appComputersList.FirstOrDefault(x => x.ComputerType.Trim().ToUpper() == comupterType[1])
                                              : appComputersList.FirstOrDefault(x => x.ComputerType.Trim().ToUpper() == comupterType[0]);

                    minimumReqiredApplications.Add(appCoumpter.UserID);

                    keyValuePairs.Add(appCoumpter.ComputerID, appCoumpter.UserID);
                    keyValuePairs.Add(appComputerWithDiffCompType.ComputerID, appComputerWithDiffCompType.UserID);
                }

                //Below code covers Example 2 where same user ID having same application installed on (2 desktop or 2 laptop) or only (1 desktop or 1 laptop)
                else if (!keyValuePairs.ContainsKey(appCoumpter.ComputerID))
                {
                    minimumReqiredApplications.Add(appComputersList.FirstOrDefault(x => x.UserID == appCoumpter.UserID).UserID);
                }

            }

            minimumReqiredApplications.AddRange(duplicateUserIDs.Select(x => x.UserID));
            return minimumReqiredApplications.Count();
        }
        #endregion Minimum Application required


        #region Methods are for test purpose only
        public int GetDuplicateRecords(IList<AppComputer> appComputerList, out List<long> computerIds)
        {
            computerIds = new List<long>();
            var duplicateRec = appComputerList.GroupBy(x => new { x.UserID, x.ComputerID })
                                .Where(group => group.Count() > 1)
                                .Select(group => new { group.Key.ComputerID, group.Key.UserID });

            computerIds.AddRange(duplicateRec.Select(x => x.ComputerID));
            return duplicateRec.Count();
        }

        public int GetSameCustomerHavingLapAndDeskRecord(IList<AppComputer> appComputers, out List<long> computerIds)
        {
            computerIds = new List<long>();
            int count = 0;
            IDictionary<long, int> keyValuePairs = new Dictionary<long, int>();
            List<AppComputer> appComputersList = new List<AppComputer>();

            var duplicateUserIDs = appComputers
                                  .GroupBy(x => new { x.UserID, x.ComputerID })
                                  .Where(group => group.Count() > 1)
                                  .Select(group => new { group.Key.ComputerID, group.Key.UserID });

            //Filter the duplicate values and get list of the information 
            var appCoumpterInfo = appComputers.Where(x => !duplicateUserIDs.Any(y => y.ComputerID == x.ComputerID));

            foreach (var appCoumpter in appCoumpterInfo)
            {

                appComputersList = appCoumpterInfo
                                   .Where(x => x.UserID == appCoumpter.UserID && !keyValuePairs.ContainsKey(x.ComputerID))
                                   .ToList();


                if ((appComputersList.Any(x => x.ComputerType.ToUpper() == comupterType[0]) && appComputersList.Any(x => x.ComputerType.ToUpper() == comupterType[1])))
                {
                    //Below query is used to avoid multipe mapping to on computer type
                    //Ex: same userID having 2 desktop and 2 laptop then it should count only 2 for that user.
                    AppComputer appComputerWithDiffCompType = appCoumpter.ComputerType.Trim().ToUpper() == comupterType[0] ?
                                              appComputersList.FirstOrDefault(x => x.ComputerType.Trim().ToUpper() == comupterType[1])
                                              : appComputersList.FirstOrDefault(x => x.ComputerType.Trim().ToUpper() == comupterType[0]);

                    count++;

                    keyValuePairs.Add(appCoumpter.ComputerID, appCoumpter.UserID);
                    keyValuePairs.Add(appComputerWithDiffCompType.ComputerID, appComputerWithDiffCompType.UserID);
                }
            }
            computerIds.AddRange(keyValuePairs.Keys);
            return count;
        }

        public List<long> GetRecordsWithMultipleOrSingleComputerType(IList<AppComputer> appComputers, IList<long> userToExclude)
        {

            List<long> customerIdList = new List<long>();
            var appCoumpterInfo = appComputers.Where(x => !userToExclude.Contains(x.ComputerID));

            foreach (var appCoumpter in appCoumpterInfo)
            {
                customerIdList.Add(appCoumpter.ComputerID);
            }
            return customerIdList;
        }

        #endregion Methods are for test purpose only
    }
}
