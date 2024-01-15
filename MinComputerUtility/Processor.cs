using MinComputerUtility.AppRepositories;
using MinComputerUtility.BusinessLogic;
using MinComputerUtility.Log;
using MinComputerUtility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinComputerUtility
{
    public static class Processor
    {
        private static StringBuilder _messageLog = new StringBuilder();

        public static void Process()
        {
            IRepository app = new AppRepository();
            BusinessDataLogic businessDataLogic = new BusinessDataLogic();
            try
            {
                Console.WriteLine("Please enter the application number:");
                int appID = Convert.ToInt32(Console.ReadLine());
                LogMessage($"Intiated Fetch for application number: {appID}");
                IList<AppComputer> applicationInfo = app.GetApplicationComputerDetails(appID);
                LogMessage($"Fetch completed for application number: {appID} and Total data count: {applicationInfo.Count()}");
                LogMessage($"Processing minimum copies of application required: ");
                int minimumApplicationCount = businessDataLogic.GetMinAppCount(applicationInfo);
                LogMessage($"Process completed and minimum copies of application required: {minimumApplicationCount}");                
            }
            catch (Exception ex)
            {
                LogMessage($"Process failed due to following exception: \n{ex.Message}");
            }
            finally
            {
                Logger.WriteLog(_messageLog);
            }
        }

        
        public static void LogMessage(string message)
        {
            _messageLog.AppendLine(message);
            Console.WriteLine(message);
        }

    }
}
