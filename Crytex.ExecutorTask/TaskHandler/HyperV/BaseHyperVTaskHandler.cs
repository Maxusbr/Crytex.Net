﻿using Crytex.Model.Models;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public abstract class BaseHyperVTaskHandler : BaseTaskHandler
    {
        protected IHyperVControl _hyperVControl;

        
        protected BaseHyperVTaskHandler(BaseTask task, IHyperVControl hyperVControl, string hostName) : base(task, hostName)
        {
            this._hyperVControl = hyperVControl;
        }
    }
}
