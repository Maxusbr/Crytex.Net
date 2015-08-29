using Crytex.Model.Models;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public abstract class BaseVmWareTaskHandler : BaseTaskHandler
    {
        protected IVmWareControl _vmWareControl;

        protected BaseVmWareTaskHandler(BaseTask task, IVmWareControl vmWareControl): base(task)
        {
            this._vmWareControl = vmWareControl;
        }
    }
}
