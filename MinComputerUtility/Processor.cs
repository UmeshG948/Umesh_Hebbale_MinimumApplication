using MinComputerUtility.AppRepositories;
using MinComputerUtility.BusinessLogic;
using MinComputerUtility.LogHandler;
using MinComputerUtility.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
                Console.WriteLine("Please enter the file path you want to processs for:");
                string filePath = Console.ReadLine();
                if (!filePath.Contains(".csv"))
                {
                    filePath = GetFilePath(filePath);
                }

                Console.WriteLine("Please enter the application number:");
                int appID = Convert.ToInt32(Console.ReadLine());
                LogMessage($"Intiated Fetch for application number: {appID}\n");
                IList<AppComputer> applicationInfo = app.GetApplicationComputerDetails(appID, filePath);
                LogMessage($"Fetch completed for application number: {appID} and Total data count: {applicationInfo.Count()}\n");
                LogMessage($"Processing minimum copies of application required: ");
                int minimumApplicationCount = businessDataLogic.GetMinAppCount(applicationInfo);
                LogMessage($"Process completed and minimum copies of application required: {minimumApplicationCount}\n");
            }
            catch (FileNotFoundException ex)
            {
                LogMessage($"File not found in the provided location: {ex.Message}");
                Console.WriteLine("\nPlease enter the valid path.");
                Process();
            }
            catch (Exception ex)
            {
                LogMessage($"Process failed due to following exception: \n{ex.Message}");
                throw;
            }
            finally
            {
                LogHanlder.WriteLog(_messageLog);
            }
        }

        private static string GetFilePath(string filePath)
        {
            IDictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            int count = 0;
            string[] allCSVFileList = Directory.GetFiles(@"" + filePath, "*.csv");
            Console.WriteLine($"Index   || File Path          \n___________________________________________");
            foreach (var file in allCSVFileList)
            {
                keyValuePairs.Add(++count, file);
                Console.WriteLine($"{keyValuePairs.FirstOrDefault(x => x.Key == count).Key}   || {keyValuePairs.FirstOrDefault(x => x.Key == count).Value}");
            }
            Console.WriteLine("Select the file Index you want to process from above list:");
            int filePathIndex = Convert.ToInt32(Console.ReadLine());
            if (keyValuePairs.ContainsKey(filePathIndex))
            {
                filePath = keyValuePairs.FirstOrDefault(x => x.Key == filePathIndex).Value;
                LogMessage($"Selected file Path: {filePath}");
            }
            else
            {
                LogMessage($"Index does not exist is current path");
                Console.WriteLine("Give a valid index.");
                GetFilePath(filePath);
            }
            return filePath;
        }

        public static void LogMessage(string message)
        {
            _messageLog.AppendLine(message+"\n");
            Console.WriteLine(message+"\n");
        }

    }
}
