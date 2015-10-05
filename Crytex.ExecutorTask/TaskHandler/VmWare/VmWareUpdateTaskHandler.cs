using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareUpdateTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareUpdateTaskHandler(TaskV2 task, IVmWareControl vmWareControl, string hostName)
            : base(task, vmWareControl, hostName) { }
        
        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Update task vmware");
            return new TaskExecutionResult();
        }
    }
}
