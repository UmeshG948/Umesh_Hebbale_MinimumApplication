using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinComputerUtility.LogHandler
{
    public static class LogHanlder
    {
        public static void WriteLog(StringBuilder message)
        {
            string logFilePath = ConfigurationSettings.AppSettings["LogFilePath"] + @"\LogMinComp_Count-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            FileInfo logFile = new FileInfo(logFilePath);
            DirectoryInfo logDirectory = new DirectoryInfo(logFile.DirectoryName);
            if (!logDirectory.Exists)
                logDirectory.Create();
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    Console.WriteLine($"Log file is generated in following path {logFilePath}");
                    log.WriteLine(message);
                }
            }
        }
    }
}
