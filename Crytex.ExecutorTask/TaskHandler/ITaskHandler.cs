using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.TaskHandler
{
    public interface ITaskHandler
    {
        BaseTask TaskEntity { get; }

        event EventHandler<BaseTask> ProcessingStarted;
        TaskExecutionResult Execute();
    }
}
