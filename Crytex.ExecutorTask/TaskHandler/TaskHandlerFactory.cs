using Crytex.ExecutorTask.TaskHandler.HyperV;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerFactory
    {
        public ITaskHandler GetHandler(BaseTask task)
        {
            return new CreateVmTaskHandler((CreateVmTask)task);
            //return new CreateVmTaskHandler((CreateVmTask)task);
        }
    }
}
