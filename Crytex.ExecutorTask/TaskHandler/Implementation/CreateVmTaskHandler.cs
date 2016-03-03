using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Virtualization.Base;
using System.Linq;
using System.Threading;
using Crytex.Virtualization.Base.InfoAboutVM;

namespace Crytex.ExecutorTask.TaskHandler
{
    internal class CreateVmTaskHandler : BaseNewTaskHandler, ITaskHandler
    {
        private IOperatingSystemsService _operatingSystemsService;
        private readonly string _deafultVmNetworkName;

        public CreateVmTaskHandler(IOperatingSystemsService operatingSystemService, TaskV2 task, IProviderVM virtualizationProvider,
            Guid virtualizationServerEntityId, string deafultVmNetworkName) : base(task, virtualizationProvider, virtualizationServerEntityId)
        {
            this._operatingSystemsService = operatingSystemService;
            _deafultVmNetworkName = deafultVmNetworkName;
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

                //add default network
                newVm.Networks.NetAdapter.Add(new NetworkInfo(_deafultVmNetworkName));

                var modifyResult = newVm.Modify();
                if (modifyResult.IsError)
                {
                    throw new ApplicationException(modifyResult.ErrorMessage);
                }

                newVm.Start(true);

                // костыль для обновления NetworkAdatapter
                Thread.Sleep(60000);
                newVm = this.VirtualizationProvider.GetMachinesByName(machineName);

                var osType = this.MapOsType(os.Family);
                newVm.UserIdentification(os.DefaultAdminName, os.DefaultAdminPassword, osType);
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
                taskExecutionResult.GuestOsPassword = newPassword;
                taskExecutionResult.Success = true;
            }
            catch (Exception ex)
            {
                taskExecutionResult.Success = false;
                taskExecutionResult.ErrorMessage = ex.Message;
            }

            return taskExecutionResult;
        }

        private VMGuestOperationType MapOsType(OperatingSystemFamily family)
        {
            switch (family)
            {
                case OperatingSystemFamily.Ubuntu:
                    return VMGuestOperationType.Linux;
                case OperatingSystemFamily.Windows2012:
                    return VMGuestOperationType.Windows;
            }

            throw new ApplicationException($"OsFamily {family} is not supported yet.");
        }
    }
}
