using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class PendingTaskHandlerBox
    {
        public IEnumerable<ITaskHandler> VmWareHandlers { get; set; }
        public IEnumerable<ITaskHandler> HyperVHandlers { get; set; }
    }
}
