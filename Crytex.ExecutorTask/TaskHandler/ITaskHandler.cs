using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    public interface ITaskHandler
    {
        TaskV2 TaskEntity { get; }
        TaskExecutionResult Execute();

        event EventHandler<TaskV2> ProcessingStarted;
        event EventHandler<TaskExecutionResult> ProcessingFinished;
    }
}
