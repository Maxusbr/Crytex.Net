using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVStandartTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVStandartTaskHandler(StandartVmTask task, IHyperVControl hyperVControl, string hostName)
            : base(task, hyperVControl, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Standart task");
            var taskExecutionResult = new TaskExecutionResult();
            try
            {
                var task = (StandartVmTask)this.TaskEntity;
                var vmName = task.VmId.ToString();
                switch (task.TaskType)
                {
                    case TypeStandartVmTask.Start:
                        this._hyperVControl.StartVm(vmName);
                        break;
                    case TypeStandartVmTask.Stop:
                        this._hyperVControl.StopVm(vmName);
                        break;
                    case TypeStandartVmTask.Remove:
                        this._hyperVControl.RemoveVm(vmName);
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
