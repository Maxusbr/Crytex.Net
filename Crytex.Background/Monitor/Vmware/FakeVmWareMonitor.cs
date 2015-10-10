using System;
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
            Array values = Enum.GetValues(typeof(VmPowerState));
            Random random = new Random();
            var machine = new VmWareVirtualMachine
            {
                Name = "VmWareMachine",
                CpuUsage = 1,
                Uptime = 1,
                RamUsage = 1,
                State = (VmPowerState)values.GetValue(random.Next(values.Length))
        };

            return machine;
        }
    }
}
