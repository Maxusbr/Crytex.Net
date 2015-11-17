using Crytex.ExecutorTask.Config;
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
        private IExecutorTaskConfig _config;
        
        public HyperVControl(IHyperVProvider hyperVProvider, IExecutorTaskConfig config)
        {
            this._hyperVProvider = hyperVProvider;
            this._config = config;
        }

        public Guid CreateVm(TaskV2 task, ServerTemplate template)
        {
            this._hyperVProvider.Connect();
            var machineGuid = Guid.NewGuid();
            var machineName = machineGuid.ToString();
            var taskOptions = task.GetOptions<CreateVmOptions>();
            var createMachineRes = this._hyperVProvider.CreateVm(machineName, (uint)taskOptions.Ram * 1024);

            if (createMachineRes.IsSuccess)
            {
                // Modify machine provessor count
                this._hyperVProvider.ModifyProcessorVm(createMachineRes.Vm, (uint)taskOptions.Cpu);
                
                // Copy vhd to new machine
                var templateDriveRoot = this._config.GetHyperVTemplateDriveRoot();
                var systemDrivePath = templateDriveRoot + template.Name;
                var vhdService = this._hyperVProvider.GetVhdService();
                var vmDriveRoot = this._config.GetHyperVVmDriveRoot();
                var newMachineDrivePath = vmDriveRoot + machineName;
                vhdService.CopyItem(systemDrivePath, newMachineDrivePath);

                // Resize copied vhd if needed
                var machineVhd = vhdService.GetVhd(newMachineDrivePath);
                if (machineVhd.VirtualHardDisk.FileSize != (uint)taskOptions.Hdd)
                {
                    vhdService.ResizeVhd(machineVhd.VirtualHardDisk, (uint)taskOptions.Hdd);
                }
                
                // Attach disk to machine
                vhdService.AttachVhd(createMachineRes.Vm, newMachineDrivePath);

                // Start vm
                this._hyperVProvider.Start(createMachineRes.Vm);
            }
            else
            {
                throw new CreateVmException(createMachineRes.MessageError);
            }

            return machineGuid;
        }


        public void UpdateVm(TaskV2 updateVmTask)
        {
            this._hyperVProvider.Connect();
            var taskOptions = updateVmTask.GetOptions<UpdateVmOptions>();
            if (this._hyperVProvider.IsVmExist(updateVmTask.ResourceId.ToString()))
            {
                var machine = this._hyperVProvider.GetVmByName(updateVmTask.ResourceId.ToString());
                var ramMb = (uint)taskOptions.Ram * 1024;
                this._hyperVProvider.ModifyMemoryVm(machine, false, ramMb, ramMb, ramMb);
                this._hyperVProvider.ModifyProcessorVm(machine, (uint)taskOptions.Cpu);
            }
            else
            {
                throw new InvalidIdentifierException(string.Format("Virtual machine with name {0} doesnt exist on this host",
                    updateVmTask.ResourceId.ToString()));
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
