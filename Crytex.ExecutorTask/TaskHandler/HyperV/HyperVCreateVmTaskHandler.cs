using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVCreateVmTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVCreateVmTaskHandler(CreateVmTask task, IHyperVControl hyperVControl) : base(task, hyperVControl) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task");
            return new TaskExecutionResult();
        }
    }
}
