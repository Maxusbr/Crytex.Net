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
        private IServerTemplateService _serverTemplateService;

        public VmWareCreateTaskHandler(TaskV2 task, IVmWareControl vmWareControl,
            IServerTemplateService serverTemplateService, string hostName)
            :base(task, vmWareControl, hostName)
        {
            this._serverTemplateService = serverTemplateService;
        }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task VmWare");
            var taskExecutionResult = new CreateVmTaskExecutionResult();
            try
            {
                var serverTemplateId = this.TaskEntity.GetOptions<CreateVmOptions>().ServerTemplateId;
                var serverTemplate = this._serverTemplateService.GetById(serverTemplateId);
                var createResult = this._vmWareControl.CreateVm(this.TaskEntity, serverTemplate);
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
