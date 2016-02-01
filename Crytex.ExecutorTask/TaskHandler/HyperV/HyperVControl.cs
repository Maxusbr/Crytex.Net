using Crytex.ExecutorTask.Config;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using HyperVRemote;
using System;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

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

        public CreateVmResult CreateVm(TaskV2 task, OperatingSystem os)
        {
            var result = new CreateVmResult();

            this._hyperVProvider.Connect();
            var taskOptions = task.GetOptions<CreateVmOptions>();
            var machineGuid = taskOptions.UserVmId;
            var machineName = machineGuid.ToString();
            var createMachineRes = this._hyperVProvider.CreateVm(machineName, (uint)taskOptions.Ram * 1024);

            if (createMachineRes.IsSuccess)
            {
                // Modify machine provessor count
                this._hyperVProvider.ModifyProcessorVm(createMachineRes.Vm, (uint)taskOptions.Cpu);
                
                // Copy vhd to new machine
                var templateDriveRoot = this._config.GetHyperVTemplateDriveRoot();
                var systemDrivePath = templateDriveRoot + os.Name;
                var vhdService = this._hyperVProvider.GetVhdService();
                var vmDriveRoot = this._config.GetHyperVVmDriveRoot();
                var newMachineDrivePath = vmDriveRoot + machineName;
                vhdService.CopyItem(systemDrivePath, newMachineDrivePath);

                // Resize copied vhd if needed
                var machineVhd = vhdService.GetVhd(newMachineDrivePath);
                if (machineVhd.VirtualHardDisk.FileSize != (uint)taskOptions.HddGB)
                {
                    vhdService.ResizeVhd(machineVhd.VirtualHardDisk, (uint)taskOptions.HddGB);
                }
                
                // Attach disk to machine
                vhdService.AttachVhd(createMachineRes.Vm, newMachineDrivePath);

                // Start vm
                this._hyperVProvider.Start(createMachineRes.Vm);

                // Get vm IP addresses
                var adapters = this._hyperVProvider.GetNetworkService().GetNetAdaptersByVm(createMachineRes.Vm).NetAdapters;
                result.NetworkAdapters = adapters;
            }
            else
            {
                throw new CreateVmException(createMachineRes.MessageError);
            }

            result.MachineGuid = machineGuid;
            return result;
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
            this._hyperVProvider.Connect();
            if (this._hyperVProvider.IsVmExist(machineName))
            {
                var machine = this._hyperVProvider.GetVmByName(machineName);
                this._hyperVProvider.Start(machine);
            }
            else
            {
                ThrowInvalidIdentifierException(machineName);
            }
        }

        public void StopVm(string machineName)
        {
            this._hyperVProvider.Connect();
            if (this._hyperVProvider.IsVmExist(machineName))
            {
                var machine = this._hyperVProvider.GetVmByName(machineName);
                this._hyperVProvider.Stop(machine);
            }
            else
            {
                ThrowInvalidIdentifierException(machineName);
            }
        }

        public void RemoveVm(string machineName)
        {
            this._hyperVProvider.Connect();
            if (this._hyperVProvider.IsVmExist(machineName))
            {
                var machine = this._hyperVProvider.GetVmByName(machineName);
                this._hyperVProvider.RemoveVm(machine);
            }
            else
            {
                ThrowInvalidIdentifierException(machineName);
            }
        }

        private void ThrowInvalidIdentifierException(string machineName)
        {
            throw new InvalidIdentifierException(string.Format("Virtual machine with name {0} doesnt exist on this host",
                machineName));
        }

        public Guid BackupVm(TaskV2 taskEntity)
        {
            throw new NotImplementedException();
        }
    }
}
