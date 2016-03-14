using Crytex.Model.Models;
using Crytex.Virtualization.Base;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    internal abstract class BaseTaskHandler : ITaskHandler
    {
        public TaskV2 TaskEntity { get; protected set; }

        public event EventHandler<TaskExecutionResult> ProcessingFinished;
        public event EventHandler<TaskV2> ProcessingStarted;

        protected abstract TaskExecutionResult ExecuteLogic();
        
        protected BaseTaskHandler(TaskV2 task)
        {
            this.TaskEntity = task;
        }

        public TaskExecutionResult Execute()
        {
            if (this.ProcessingStarted != null)
            {
                this.ProcessingStarted.Invoke(this, this.TaskEntity);
            }

            var taskResult = this.ExecuteLogic();
            taskResult.TaskEntity = this.TaskEntity;

            if (this.ProcessingFinished != null)
            {
                this.ProcessingFinished(this, taskResult);
            }

            return taskResult;
        }
    }
}