using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    public abstract class BaseTaskHandler
    {
        public BaseTask TaskEntity { get; protected set; }

        public event EventHandler<BaseTask> ProcessingStarted;
        public event EventHandler<TaskExecutionResult> ProcessingFinished;

        protected abstract TaskExecutionResult ExecuteLogic();

        protected BaseTaskHandler(BaseTask task)
        {
            this.TaskEntity = task;
        }

        public TaskExecutionResult Execute()
        {
            this.ProcessingStarted.Invoke(this, this.TaskEntity);
            var taskResult = this.ExecuteLogic();
            this.ProcessingFinished(this, taskResult);

            return taskResult;
        }
    }
}
