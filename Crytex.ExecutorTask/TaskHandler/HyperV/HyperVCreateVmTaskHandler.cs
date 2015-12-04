using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVCreateVmTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        private IServerTemplateService _serverTemplateService;
        public HyperVCreateVmTaskHandler(TaskV2 task, IHyperVControl hyperVControl, 
            IServerTemplateService serverTemplateService, string hostName) 
            : base(task, hyperVControl, hostName) 
        {
            this._serverTemplateService = serverTemplateService;
        }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task");
            var taskExecutionResult = new CreateVmTaskExecutionResult();
            try
            {
                var serverTemplateId = this.TaskEntity.GetOptions<CreateVmOptions>().ServerTemplateId;
                var serverTemplate = this._serverTemplateService.GetById(serverTemplateId);
                var machineGuid = this._hyperVControl.CreateVm(this.TaskEntity, serverTemplate);
                taskExecutionResult.Success = true;
                taskExecutionResult.MachineGuid = machineGuid;
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
