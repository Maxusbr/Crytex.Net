using Crytex.Model.Models;
using VmWareRemote.Model;

namespace Crytex.Background.Monitor.Vmware
{
    class FakeVmWareMonitorFactory : IVmWareMonitorFactory
    {
        public IVmWareMonitor CreateVmWareVMonitor(VmWareVCenter vCenter)
        {
            var configuration = new VmWareConfiguration(vCenter.UserName, vCenter.Password, vCenter.ServerAddress);
            var vmWareProvider = new FakeVmWareProvider(configuration); // fake realization provider
            var control = new FakeVmWareMonitor(vmWareProvider); // fake realization control

            return control;
        }
    }
}
