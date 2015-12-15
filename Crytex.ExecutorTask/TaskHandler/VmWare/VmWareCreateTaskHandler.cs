using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareCreateTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        private IOperatingSystemsService _operatingSystemsService;
        public VmWareCreateTaskHandler(TaskV2 task, IVmWareControl hyperVControl,
            IOperatingSystemsService operatingSystemsService, string hostName) 
            : base(task, hyperVControl, hostName) 
        {
            this._operatingSystemsService = operatingSystemsService;
        }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task VmWare");
            var taskExecutionResult = new CreateVmTaskExecutionResult();
            try
            {
                var osId = this.TaskEntity.GetOptions<CreateVmOptions>().OperatingSystemId;
                var os = this._operatingSystemsService.GetById(osId);
                var createResult = this._vmWareControl.CreateVm(this.TaskEntity, os);
                taskExecutionResult.Success = true;
                taskExecutionResult.MachineGuid = createResult.MachineGuid;
                taskExecutionResult.GuestOsPassword = createResult.GuestOsAdminPassword;

                var ipAddresses = createResult.IpAddresses.Select(info =>
                    new VmIpAddress
                    {
                        IPv4 = info.IPv4,
                        IPv6 = info.IPv6,
                        NetworkName = info.NetworkName,
                        MAC = info.MAC
                    }
                );

                taskExecutionResult.IpAddresses = ipAddresses;
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
