using MinComputerUtility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinComputerUtility.AppRepositories
{
    public interface IRepository
    {
        IList<AppComputer> GetApplicationComputerDetails(int appId);
    }

}
