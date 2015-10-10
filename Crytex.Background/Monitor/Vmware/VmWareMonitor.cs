using VmWareRemote.Interface;
using VmWareRemote.Model;

namespace Crytex.Background.Monitor.Vmware
{
    public class VmWareMonitor : IVmWareMonitor
    {
        private IVmWareProvider _vmWareProvider;

        public VmWareMonitor(IVmWareProvider vmWareProvider)
        {
            this._vmWareProvider = vmWareProvider;
        }

        VmWareVirtualMachine IVmWareMonitor.GetVmByName(string vmName)
        {
            return _vmWareProvider.GetMachineByName(vmName);
        }
    }
}
