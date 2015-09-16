using Crytex.Model.Models;
using HyperVRemote;
using System;
using System.Collections.Generic;
namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVControl : IHyperVControl
    {
        private IHyperVProvider _hyperVProvider;
        
        public HyperVControl(IHyperVProvider hyperVProvider)
        {
            this._hyperVProvider = hyperVProvider;
        }

        public Guid CreateVm(CreateVmTask task)
        {
            this._hyperVProvider.Connect();
            var machineGuid = Guid.NewGuid();
            var machine = this._hyperVProvider.CreateMachine(machineGuid.ToString());
            var machineSettings = new List<IMachineSetting>()
            {
                new MemoryMachineSetting(machine) { MinRam = (ulong)task.Ram },
                new ProcessorMachineSetting(machine) { NumberOfVirtualProcessors = (ulong)task.Cpu}

            };
            this._hyperVProvider.ModifyMachine(machine, machineSettings);

            return machineGuid;
        }
    }
}
