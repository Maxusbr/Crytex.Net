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
            Random random = new Random();

            PSObject name = new PSObject();
            name.Properties.Add(new PSNoteProperty("CPUUsage", random.Next(1, 16)));
            name.Properties.Add(new PSNoteProperty("MemoryAssigned", Convert.ToInt64(random.Next(512,16000))));
            name.Properties.Add(new PSNoteProperty("Name", vmName));
            name.Properties.Add(new PSNoteProperty("Status", "Status"));
            name.Properties.Add(new PSNoteProperty("Uptime", TimeSpan.MinValue));

            string[] values = new string[3] {"Running", "Disabled", "Restart"};

            Random randomTwo = new Random((int) DateTime.Now.Ticks);
            name.Properties.Add(new PSNoteProperty("State", values.GetValue(randomTwo.Next(values.Length))));

            return new HyperVMachine(name);
        }
    }
}
