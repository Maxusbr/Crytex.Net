using Crytex.Model.Models;
using HyperVRemote.Source.Implementation;

namespace Crytex.Background.Monitor.HyperV
{
    class FakeHyperVMonitorFactory : IHyperVMonitorFactory
    {
        public IHyperVMonitor CreateHyperVMonitor(HyperVHost host)
        {
            var configuration = new HyperVConfiguration(host.UserName, host.Password, host.Host);
            var hyperVProvider = new FakeHyperVProvider(configuration); // fake realization provider
            var control = new FakeHyperVMonitor(hyperVProvider); // fake realization control

            return control;
        }
    }
}
