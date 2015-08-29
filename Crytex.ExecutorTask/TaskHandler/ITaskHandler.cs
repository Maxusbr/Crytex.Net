using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    public interface ITaskHandler
    {
        BaseTask TaskEntity { get; }
        TaskExecutionResult Execute();

        event EventHandler<BaseTask> ProcessingStarted;
        event EventHandler<TaskExecutionResult> ProcessingFinished;
    }
}
