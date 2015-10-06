using Crytex.Model.Models;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public abstract class BaseVmWareTaskHandler : BaseTaskHandler
    {
        protected IVmWareControl _vmWareControl;

        protected BaseVmWareTaskHandler(TaskV2 task, IVmWareControl vmWareControl, string hostName): base(task, hostName)
        {
            this._vmWareControl = vmWareControl;
        }
    }
}
