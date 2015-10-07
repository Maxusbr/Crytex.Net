using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    class VmWareRemoveVmTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareRemoveVmTaskHandler(TaskV2 task, IVmWareControl vmWareControl, string hostName)
            :base(task, vmWareControl, hostName){ }

        protected override TaskExecutionResult ExecuteLogic()
        {
            var taskExecutionResult = new TaskExecutionResult();
            try
            {
                var machineName = this.TaskEntity.GetOptions<RemoveVmOptions>().VmId.ToString();
                this._vmWareControl.RemoveVm(machineName);
                taskExecutionResult.Success = true;
            }
            catch (ApplicationException ex)
            {
                taskExecutionResult.Success = false;
                taskExecutionResult.ErrorMessage = ex.Message;
            }

            return taskExecutionResult;
        }
    }
}
