using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareUpdateTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareUpdateTaskHandler(TaskV2 task, IVmWareControl vmWareControl, string hostName)
            : base(task, vmWareControl, hostName) { }
        
        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Update task");
            var taskExecutionResult = new TaskExecutionResult();
            try
            {
                this._vmWareControl.UpdateVm(this.TaskEntity);
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
