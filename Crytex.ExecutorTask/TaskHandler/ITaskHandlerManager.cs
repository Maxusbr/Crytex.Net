using Crytex.Model.Models;
using System.Collections.Generic;

namespace Crytex.ExecutorTask.TaskHandler
{
    public interface ITaskHandlerManager
    {
        IEnumerable<ITaskHandler> GetTaskHandlers(IEnumerable<TaskV2> tasks);
    }
}
