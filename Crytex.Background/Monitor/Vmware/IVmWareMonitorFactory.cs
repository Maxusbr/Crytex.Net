using Crytex.Model.Models;

namespace Crytex.Background.Monitor.Vmware
{
    public interface IVmWareMonitorFactory
    {
        IVmWareMonitor CreateVmWareVMonitor(VmWareVCenter vCenter);
    }
}
