using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareCreateTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareCreateTaskHandler(CreateVmTask task, IVmWareControl vmWareControl) :base(task, vmWareControl){ }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task vmware");
            return new TaskExecutionResult();
        }
    }
}
