using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinComputerUtility.AppRepositories;

namespace MinComputerUtility
{ 
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Application utility process started");
            try
            {
                Processor.Process();

                Console.WriteLine("Want to process for other application number: Yes/No");
                string needToProcess = Console.ReadLine();
                if (needToProcess.ToLower().Trim() == "yes")
                {
                    Processor.Process();
                }
                Console.WriteLine("Press enter to end the process");
                Console.ReadLine();
            } 
            catch
            {
                Console.WriteLine("Job has failed due to exception please refer the logs");
            }
        }
    }
}
