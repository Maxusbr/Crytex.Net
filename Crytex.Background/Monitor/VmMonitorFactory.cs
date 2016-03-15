using Crytex.Model.Models;
using Crytex.Virtualization.Base;
using Crytex.Virtualization._VMware;

namespace Crytex.Background.Monitor
{
    class VmMonitorFactory : IVmMonitorFactory
    {
        public IVmMonitor GetVmWareMonitor(VmWareVCenter vCenter)
        {
            AutorizationInfo authData = new AutorizationInfo
            {
                UserName = vCenter.UserName,
                ServerAddress = vCenter.ServerAddress,
                UserPassword = vCenter.Password
            };
            var provider = new ProviderWMware(authData);
            var monitor = new VmMonitor(provider);

            return monitor;
        }

        public IVmMonitor GetHyperVMonitor(HyperVHost host)
        {
            throw new System.NotImplementedException();
        }
    }
}
