using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareStandartVmWareTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareStandartVmWareTaskHandler(TaskV2 task, IVmWareControl vmWareControl, string hostName)
            : base(task, vmWareControl, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            var taskExecutionResult = new TaskExecutionResult();
            try
            {
                var task = this.TaskEntity;
                var taskOptions = task.GetOptions<ChangeStatusOptions>();
                var vmName = task.ResourceId.ToString();
                switch (taskOptions.TypeChangeStatus)
                {
                    case TypeChangeStatus.Start:
                        this._vmWareControl.StartVm(vmName);
                        break;
                    case TypeChangeStatus.Stop:
                        this._vmWareControl.StopVm(vmName);
                        break;
                }

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
