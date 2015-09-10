using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytext.HyperVVirtualization
{
    public interface ISystemCenterRemoteVirtualManagerService
    {
        bool Check(SystemCenterVirtualManager manager);
        IEnumerable<HyperVHost> GetHyperVMHosts(SystemCenterVirtualManager manager);
    }
}
