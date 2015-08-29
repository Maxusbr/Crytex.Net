using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareUpdateTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareUpdateTaskHandler(UpdateVmTask task, IVmWareControl vmWareControl) : base(task, vmWareControl) { }
        
        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Update task vmware");
            return new TaskExecutionResult();
        }
    }
}
