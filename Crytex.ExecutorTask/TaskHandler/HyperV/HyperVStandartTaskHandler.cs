using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVStandartTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVStandartTaskHandler(TaskV2 task, IHyperVControl hyperVControl, string hostName)
            : base(task, hyperVControl, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Standart task");
            var taskExecutionResult = new TaskExecutionResult();
            try
            {
                var task = this.TaskEntity;
                var taskOptions = task.GetOptions<ChangeStatusOptions>();
                var vmName = taskOptions.VmId.ToString();
                switch (taskOptions.TypeChangeStatus)
                {
                    case TypeChangeStatus.Start:
                        this._hyperVControl.StartVm(vmName);
                        break;
                    case TypeChangeStatus.Stop:
                        this._hyperVControl.StopVm(vmName);
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
