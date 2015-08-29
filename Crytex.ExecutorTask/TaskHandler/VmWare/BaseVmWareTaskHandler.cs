using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
