using VmWareRemote.Model;

namespace Crytex.Background.Monitor.Vmware
{
    public interface IVmWareMonitor
    {
        VmWareVirtualMachine GetVmByName(string vmName);
    }
}
