using Crytex.Model.Exceptions;
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
            var machine = this._hyperVProvider.CreateVm(machineGuid.ToString(), (uint)task.Ram * 1024);

            this._hyperVProvider.ModifyProcessorVm(machine.Vm, (uint)task.Cpu);

            return machineGuid;
        }


        public void UpdateVm(UpdateVmTask updateVmTask)
        {
            this._hyperVProvider.Connect();
            if (this._hyperVProvider.IsVmExist(updateVmTask.VmId.ToString()))
            {
                var machine = this._hyperVProvider.GetVmByName(updateVmTask.VmId.ToString());
                var ramMb = (uint)updateVmTask.Ram * 1024;
                this._hyperVProvider.ModifyMemoryVm(machine, false, ramMb, ramMb, ramMb);
                this._hyperVProvider.ModifyProcessorVm(machine, (uint)updateVmTask.Cpu);
            }
            else
            {
                throw new InvalidIdentifierException(string.Format("Virtual machine with name {0} doesnt exist on this host",
                    updateVmTask.VmId));
            }
        }


        public void StartVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Start);
        }

        public void StopVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Stop);
        }

        public void RemoveVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Remove);
        }

        private void StandartOperationInner(string machineName, TypeStandartVmTask type)
        {
            this._hyperVProvider.Connect();
            if (this._hyperVProvider.IsVmExist(machineName))
            {
                var machine = this._hyperVProvider.GetVmByName(machineName);
                switch (type)
                {
                    case TypeStandartVmTask.Start:
                        this._hyperVProvider.Start(machine);
                        break;
                    case TypeStandartVmTask.Stop:
                        this._hyperVProvider.Stop(machine);
                        break;
                    case TypeStandartVmTask.Remove:
                        this._hyperVProvider.RemoveVm(machine);
                        break;
                }
            }
            else
            {
                throw new InvalidIdentifierException(string.Format("Virtual machine with name {0} doesnt exist on this host",
                    machineName));
            }
        }
    }
}
