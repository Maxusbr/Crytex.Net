using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVUpdateVmTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVUpdateVmTaskHandler(TaskV2 task, IHyperVControl hyperVControl, string hostName)
            : base(task, hyperVControl, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Update task");
            var taskExecutionResult = new TaskExecutionResult();
            try
            {
                this._hyperVControl.UpdateVm(this.TaskEntity);
                taskExecutionResult.Success = true;
            }
            catch
            {
                taskExecutionResult.Success = false;
            }

            return taskExecutionResult;
        }
    }
}
