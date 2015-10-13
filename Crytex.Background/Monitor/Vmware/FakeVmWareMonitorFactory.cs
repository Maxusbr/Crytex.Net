using Crytex.Model.Models;
using VmWareRemote.Model;

namespace Crytex.Background.Monitor.Vmware
{
    class FakeVmWareMonitorFactory : IVmWareMonitorFactory
    {
        public IVmWareMonitor CreateVmWareVMonitor(VmWareVCenter vCenter)
        {
            var vmWareProvider = new FakeVmWareProvider(); // fake realization provider
            var control = new FakeVmWareMonitor(vmWareProvider); // fake realization control

            return control;
        }
    }
}
