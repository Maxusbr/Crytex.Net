using Crytex.Model.Models;
using VmWareRemote.Implementations;
using VmWareRemote.Model;

namespace Crytex.Background.Monitor.Vmware
{
    class VmWareMonitorFactory : IVmWareMonitorFactory
    {
        public IVmWareMonitor CreateVmWareVMonitor(VmWareVCenter vCenter)
        {
            var vmWareProvider = new VmWareProvider(vCenter.UserName, vCenter.Password, vCenter.ServerAddress);
            var control = new VmWareMonitor(vmWareProvider);

            return control;
        }
    }
}
