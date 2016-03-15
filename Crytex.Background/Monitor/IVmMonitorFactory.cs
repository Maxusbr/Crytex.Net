using Crytex.Model.Models;

namespace Crytex.Background.Monitor
{
    public interface IVmMonitorFactory
    {
        IVmMonitor GetVmWareMonitor(VmWareVCenter vCenter);
        IVmMonitor GetHyperVMonitor(HyperVHost host);
    }
}
