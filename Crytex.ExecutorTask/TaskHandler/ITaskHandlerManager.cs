using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.TaskHandler
{
    public interface ITaskHandlerManager
    {
        PendingTaskHandlerBox GetTaskHandlers();
    }
}
