using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVCreateVmTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVCreateVmTaskHandler(TaskV2 task, IHyperVControl hyperVControl, string hostName) 
            : base(task, hyperVControl, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task");
            var taskExecutionResult = new TaskExecutionResult();
            try
            {
                var machineGuid = this._hyperVControl.CreateVm(this.TaskEntity);
                taskExecutionResult.Success = true;
                taskExecutionResult.MachineGuid = machineGuid;
            }
            catch(CreateVmException ex)
            {
                taskExecutionResult.Success = false;
                taskExecutionResult.ErrorMessage = ex.Message;
            }

            return taskExecutionResult;
        }
    }
}
