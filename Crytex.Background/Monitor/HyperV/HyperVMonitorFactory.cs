using Crytex.Model.Models;
using HyperVRemote;
using HyperVRemote.Source.Implementation;


namespace Crytex.Background.Monitor.HyperV
{
    class HyperVMonitorFactory : IHyperVMonitorFactory
    {
        public IHyperVMonitor CreateHyperVMonitor(HyperVHost host)
        {
            var configuration = new HyperVConfiguration(host.UserName, host.Password, host.Host);
            var hyperVProvider = new HyperVProvider(configuration); 
            var control = new HyperVMonitor(hyperVProvider); 

            return control;
        }
    }
}
