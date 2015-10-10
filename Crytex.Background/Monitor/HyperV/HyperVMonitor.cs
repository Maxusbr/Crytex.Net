using HyperVRemote;

namespace Crytex.Background.Monitor.HyperV
{
    public class HyperVMonitor : IHyperVMonitor
    {
        private IHyperVProvider _hyperVProvider;

        public HyperVMonitor(IHyperVProvider hyperVProvider)
        {
            this._hyperVProvider = hyperVProvider;
        }

        public HyperVMachine GetVmByName(string vmName)
        {
            return _hyperVProvider.GetVmByName(vmName);
        }
    }
}
