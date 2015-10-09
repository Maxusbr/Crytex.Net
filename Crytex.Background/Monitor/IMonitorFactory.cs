using Crytex.Background.Monitor.HyperV;
using Crytex.Background.Monitor.Vmware;
using Crytex.Model.Models;

namespace Crytex.Background.Monitor
{
    public interface IMonitorFactory
    {
        IHyperVMonitor CreateHyperVMonitor(HyperVHost host);
        IVmWareMonitor CreateVmWareVMonitor(VmWareHost host);
    }
}
