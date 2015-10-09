using HyperVRemote;

namespace Crytex.Background.Monitor.HyperV
{
    public class FakeHyperVMonitor : IHyperVMonitor
    {
        private IHyperVProvider _hyperVProvider;

        public FakeHyperVMonitor(IHyperVProvider hyperVProvider)
        {
            this._hyperVProvider = hyperVProvider;
        }

        public HyperVMachine GetVmByName(string vmName)
        {
            return _hyperVProvider.GetVmByName(vmName);
        }
    }
}
