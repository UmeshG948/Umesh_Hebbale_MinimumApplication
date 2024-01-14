using MinComputerUtility.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinComputerUtility.Log;

namespace MinComputerUtility.AppRepositories
{
    public class AppRepository : IRepository
    {
        private readonly string _filePath = Path.GetFullPath(ConfigurationSettings.AppSettings["FilePath"]);
          
        
        //Return Application data
        public IList<AppComputer> GetApplicationComputerDetails(int appId)
        {
            return GetAppComputers(appId);
        }

        /// <summary>
        /// Gets the Application Computer details
        /// </summary>
        /// <param name="appId">ApplicationID</param>
        /// <returns>List of user and computer associated to specific application</returns>
        private IList<AppComputer> GetAppComputers(int appId)
        {
            try
            {
                using (var reader = new StreamReader(_filePath))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                       
                        return csv.GetRecords<AppComputer>().Where(x => x.ApplicationID == appId).ToList();
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }

        
    }
}
