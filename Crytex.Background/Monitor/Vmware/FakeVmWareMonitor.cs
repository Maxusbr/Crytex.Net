using VmWareRemote.Interface;
using VmWareRemote.Model;

namespace Crytex.Background.Monitor.Vmware
{
    public class FakeVmWareMonitor : IVmWareMonitor
    {
        private IVmWareProvider _vmWareProvider;

        public FakeVmWareMonitor(IVmWareProvider vmWareProvider)
        {
            this._vmWareProvider = vmWareProvider;
        }

        VmWareVirtualMachine IVmWareMonitor.GetVmByName(string vmName)
        {
            return _vmWareProvider.GetMachineByName(vmName);
        }
    }
}
