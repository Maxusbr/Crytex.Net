using HyperVRemote;

namespace Crytex.Background.Monitor.HyperV
{
    public interface IHyperVMonitor
    {
        HyperVMachine GetVmByName(string vmName);
    }
}
