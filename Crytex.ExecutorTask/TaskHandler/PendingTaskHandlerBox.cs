using System.Collections.Generic;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class PendingTaskHandlerBox
    {
        public IEnumerable<ITaskHandler> VmWareHandlers { get; set; }
        public IEnumerable<ITaskHandler> HyperVHandlers { get; set; }
    }
}
