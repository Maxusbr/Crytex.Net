using System;
using System.Management.Automation;
using HyperVRemote;
using VmWareRemote.Model;

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
            PSObject name = new PSObject();
            name.Properties.Add(new PSNoteProperty("CPUUsage", 1));
            name.Properties.Add(new PSNoteProperty("MemoryAssigned", Convert.ToInt64(2)));
            name.Properties.Add(new PSNoteProperty("Name", vmName));
            name.Properties.Add(new PSNoteProperty("Status", "Status"));
            name.Properties.Add(new PSNoteProperty("Uptime", TimeSpan.MinValue));

            Array values = Enum.GetValues(typeof(VmPowerState));
            Random random = new Random();
            VmPowerState randomState = (VmPowerState)values.GetValue(random.Next(values.Length));
            name.Properties.Add(new PSNoteProperty("State", randomState.ToString("G")));

            return new HyperVMachine(name);
        }
    }
}
