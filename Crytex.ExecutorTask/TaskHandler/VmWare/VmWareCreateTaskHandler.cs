using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareCreateTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareCreateTaskHandler(TaskV2 task, IVmWareControl vmWareControl, string hostName)
            :base(task, vmWareControl, hostName){ }

        protected override TaskExecutionResult ExecuteLogic()
        {
            var taskExecutionResult = new TaskExecutionResult();
            try
            {
                var machineGuid = this._vmWareControl.CreateVm(this.TaskEntity);
                taskExecutionResult.Success = true;
                taskExecutionResult.MachineGuid = machineGuid;
            }
            catch (CreateVmException ex)
            {
                taskExecutionResult.Success = false;
                taskExecutionResult.ErrorMessage = ex.Message;
            }

            return taskExecutionResult;
        }
    }
}
