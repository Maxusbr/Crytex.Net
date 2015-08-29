using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
