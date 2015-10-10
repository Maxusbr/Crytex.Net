using Crytex.Model.Models;

namespace Crytex.Background.Monitor.HyperV
{
    public interface IHyperVMonitorFactory
    {
        IHyperVMonitor CreateHyperVMonitor(HyperVHost host);
    }
}
