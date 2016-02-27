using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Virtualization.Base;
using System.Linq;

namespace Crytex.ExecutorTask.TaskHandler
{
    internal class CreateVmTaskHandler : BaseNewTaskHandler, ITaskHandler
    {
        private IOperatingSystemsService _operatingSystemsService;

        public CreateVmTaskHandler(IOperatingSystemsService operatingSystemService, TaskV2 task, IProviderVM virtualizationProvider,
            Guid virtualizationServerEntityId) : base(task, virtualizationProvider, virtualizationServerEntityId)
        {
            this._operatingSystemsService = operatingSystemService;
        }
        
        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine($"Create task");
            var taskExecutionResult = new CreateVmTaskExecutionResult();
            try
            {
                var osId = this.TaskEntity.GetOptions<CreateVmOptions>().OperatingSystemId;
                var os = this._operatingSystemsService.GetById(osId);
                var createTaskOptions = this.TaskEntity.GetOptions<CreateVmOptions>();

                var machineGuid = createTaskOptions.UserVmId;
                var machineName = machineGuid.ToString();

                var newPassword = System.Web.Security.Membership.GeneratePassword(6, 0); 

                this.ConnectProvider();

                var vmToClone = this.VirtualizationProvider.GetMachinesByName(os.ServerTemplateName);
                vmToClone.CloneMachine(machineName);
                var newVm = this.VirtualizationProvider.GetMachinesByName(machineName);

                //modify hardware
                newVm.NumCPU = createTaskOptions.Cpu;
                newVm.Memory = createTaskOptions.Ram;
                newVm.VirtualDrives.Drives.First().ResizeDisk(createTaskOptions.HddGB);
                var modifyResult = newVm.Modify();
                if (modifyResult.IsError)
                {
                    throw new ApplicationException(modifyResult.ErrorMessage);
                }

                newVm.Start(true);
                newVm.SetNewPassword(newPassword);

                var ipAddresses = newVm.Networks.NetAdapter.Select(adp => new VmIpAddress
                {
                    IPv4 = adp.IPv4,
                    IPv6 = adp.IPv6,
                    MAC = adp.MAC,
                    NetworkName = adp.NetworkName
                });

                taskExecutionResult.IpAddresses = ipAddresses;
                taskExecutionResult.MachineGuid = machineGuid;
                taskExecutionResult.Success = true;
            }
            catch (Exception ex)
            {
                taskExecutionResult.Success = false;
                taskExecutionResult.ErrorMessage = ex.Message;
            }

            return taskExecutionResult;
        }
    }
}
