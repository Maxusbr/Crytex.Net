using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVUpdateVmTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVUpdateVmTaskHandler(UpdateVmTask task, IHyperVControl hyperVControl) : base(task, hyperVControl) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Update task");
            return new TaskExecutionResult();
        }
    }
}
