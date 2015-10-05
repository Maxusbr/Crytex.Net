using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareCreateTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareCreateTaskHandler(TaskV2 task, IVmWareControl vmWareControl, string hostName)
            :base(task, vmWareControl, hostName){ }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Create task vmware");
            return new TaskExecutionResult();
        }
    }
}
