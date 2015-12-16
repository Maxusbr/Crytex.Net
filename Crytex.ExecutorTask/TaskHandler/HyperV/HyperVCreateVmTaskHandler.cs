using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Linq;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVCreateVmTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        private IOperatingSystemsService _operatingSystemsService;
        public HyperVCreateVmTaskHandler(TaskV2 task, IHyperVControl hyperVControl, 
            IOperatingSystemsService operatingSystemsService, string hostName) 
            : base(task, hyperVControl, hostName) 
        {
            this._operatingSystemsService = operatingSystemsService;
        }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task HyperV");
            var taskExecutionResult = new CreateVmTaskExecutionResult();
            try
            {
                var osId = this.TaskEntity.GetOptions<CreateVmOptions>().OperatingSystemId;
                var os = this._operatingSystemsService.GetById(osId);
                var createResult = this._hyperVControl.CreateVm(this.TaskEntity, os);
                taskExecutionResult.Success = true;
                taskExecutionResult.MachineGuid = createResult.MachineGuid;
                taskExecutionResult.IpAddresses = createResult.NetworkAdapters.Select(adp =>
                    new VmIpAddress
                    {
                        IPv4 = adp.IPAddress_v4,
                        IPv6 = adp.IPAddress_v6,
                        MAC = adp.MacAddress,
                        NetworkName = adp.SwitchName
                    }
                );
            }
            catch (Exception ex) when (ex is CreateVmException || ex is InvalidIdentifierException) 
            {
                taskExecutionResult.Success = false;
                taskExecutionResult.ErrorMessage = ex.Message;
            }

            return taskExecutionResult;
        }
    }
}
