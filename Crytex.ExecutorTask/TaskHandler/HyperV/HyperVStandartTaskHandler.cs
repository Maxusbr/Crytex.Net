using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVStandartTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVStandartTaskHandler(StandartVmTask task, IHyperVControl hyperVControl) : base(task, hyperVControl) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Standart task");
            return new TaskExecutionResult();
        }
    }
}
