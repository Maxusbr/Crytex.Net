using Crytex.Model.Models;
using VmWareRemote.Implementations;
using VmWareRemote.Model;

namespace Crytex.Background.Monitor.Vmware
{
    class VmWareMonitorFactory : IVmWareMonitorFactory
    {
        public IVmWareMonitor CreateVmWareVMonitor(VmWareVCenter vCenter)
        {
            var configuration = new VmWareConfiguration(vCenter.UserName, vCenter.Password, vCenter.ServerAddress);
            var vmWareProvider = new VmWareProvider(configuration);
            var control = new VmWareMonitor(vmWareProvider);

            return control;
        }
    }
}
