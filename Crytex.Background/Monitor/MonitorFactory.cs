using Crytex.Background.Monitor.HyperV;
using Crytex.Background.Monitor.Vmware;
using Crytex.Model.Models;
using HyperVRemote.Source.Implementation;
using VmWareRemote.Implementations;
using VmWareRemote.Model;

namespace Crytex.Background.Monitor
{
    class MonitorFactory : IMonitorFactory
    {
        public IHyperVMonitor CreateHyperVMonitor(HyperVHost host)
        {
            var configuration = new HyperVConfiguration(host.UserName, host.Password, host.Host);
            var hyperVProvider = new FakeHyperVProvider(configuration); // fake realization provider
            var control = new FakeHyperVMonitor(hyperVProvider); // fake realization control

            return control;
        }

        public IVmWareMonitor CreateVmWareVMonitor(VmWareHost host)
        {
            var configuration = new VmWareConfiguration("username", "password", host.Host);
            var vmWareProvider = new VmWareProvider(configuration); // fake realization provider
            var control = new FakeVmWareMonitor(vmWareProvider); // fake realization control

            return control;
        }
    }
}
