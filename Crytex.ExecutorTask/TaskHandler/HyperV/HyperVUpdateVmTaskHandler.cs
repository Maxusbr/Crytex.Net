using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVUpdateVmTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVUpdateVmTaskHandler(UpdateVmTask task, IHyperVControl hyperVControl, string hostName)
            : base(task, hyperVControl, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Update task");
            return new TaskExecutionResult();
        }
    }
}
