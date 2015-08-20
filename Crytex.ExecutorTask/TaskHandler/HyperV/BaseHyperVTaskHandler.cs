using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.ExecutorTask.TaskHandler;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public abstract class BaseHyperVTaskHandler : BaseTaskHandler
    {
        protected IHyperVControl _hyperVControl;

        
        protected BaseHyperVTaskHandler(BaseTask task, IHyperVControl hyperVControl) : base(task)
        {
            this._hyperVControl = hyperVControl;
        }
    }
}
