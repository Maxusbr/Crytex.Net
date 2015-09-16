﻿using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using HyperVRemote;
using HyperVRemote.Source.Interface;
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


        public void UpdateVm(UpdateVmTask updateVmTask)
        {
            this._hyperVProvider.Connect();
            if (this._hyperVProvider.IsVmExist(updateVmTask.VmId.ToString()))
            {
                var machine = this._hyperVProvider.GetMachineByName(updateVmTask.VmId.ToString());
                var machineSettings = new List<IMachineSetting>()
                {
                    new MemoryMachineSetting(machine) { MinRam = (ulong)updateVmTask.Ram },
                    new ProcessorMachineSetting(machine) { NumberOfVirtualProcessors = (ulong)updateVmTask.Cpu}

                };
                this._hyperVProvider.ModifyMachine(machine, machineSettings);
            }
            else
            {
                throw new InvalidIdentifierException(string.Format("Virtual machine with name {0} doesnt exist on this host",
                    updateVmTask.VmId));
            }
        }


        public void StartVm(string machineName)
        {
            this.StandartOperationInner(machineName, this._hyperVProvider.Start);
        }

        public void StopVm(string machineName)
        {
            this.StandartOperationInner(machineName, this._hyperVProvider.Stop);
        }

        public void RemoveVm(string machineName)
        {
            this.StandartOperationInner(machineName, this._hyperVProvider.DeleteMachine);
        }

        private void StandartOperationInner(string machineName, Action<IHyperVMachine> action)
        {
            this._hyperVProvider.Connect();
            if (this._hyperVProvider.IsVmExist(machineName))
            {
                var machine = this._hyperVProvider.GetMachineByName(machineName);
                action.Invoke(machine);
            }
            else
            {
                throw new InvalidIdentifierException(string.Format("Virtual machine with name {0} doesnt exist on this host",
                    machineName));
            }
        }
    }
}
