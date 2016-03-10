using Crytex.Model.Models;

namespace Crytex.Background.Monitor.Fake
{
    class FakeVmMonitorFactory : IVmMonitorFactory
    {
        public IVmMonitor GetVmWareMonitor(VmWareVCenter vCenter)
        {
            return new FakeVmMonitor();
        }

        public IVmMonitor GetHyperVMonitor(HyperVHost host)
        {
            return new FakeVmMonitor();
        }
    }
}
