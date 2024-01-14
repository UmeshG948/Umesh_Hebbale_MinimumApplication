using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinComputerUtility.Model
{
    public class AppComputer
    {
        public long ComputerID { get; set; }
        public int UserID { get; set; }
        public int ApplicationID { get; set; }
        public string ComputerType { get; set; }
        public string Comment { get; set; }
    }
}
