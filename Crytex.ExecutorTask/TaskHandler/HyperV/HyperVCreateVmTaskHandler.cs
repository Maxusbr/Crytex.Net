using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVCreateVmTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVCreateVmTaskHandler(CreateVmTask task, IHyperVControl hyperVControl, string hostName) 
            : base(task, hyperVControl, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task");
            var taskExecutionResult = new TaskExecutionResult()
            {
                TaskEntity = this.TaskEntity
            };
            try
            {
                var machineGuid = this._hyperVControl.CreateVm(this.TaskEntity as CreateVmTask);
                taskExecutionResult.Success = true;
                taskExecutionResult.MachineGuid = machineGuid;
            }
            catch
            {
                taskExecutionResult.Success = false;
            }

            return taskExecutionResult;
        }
    }
}
