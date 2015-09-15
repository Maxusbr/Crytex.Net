using Crytex.Model.Models;
using Crytext.HyperVVirtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Test.FakeImplementations
{
    public class FakeSystemCenterRemoteVirtualManagerService : ISystemCenterRemoteVirtualManagerService
    {
        public List<HyperVHost> RemoteHosts { get; set; }

        public FakeSystemCenterRemoteVirtualManagerService()
        {
            this.RemoteHosts = new List<HyperVHost>();
        }

        public bool Check(SystemCenterVirtualManager manager)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HyperVHost> GetHyperVMHosts(SystemCenterVirtualManager manager)
        {
            return this.RemoteHosts;
        }
    }
}
