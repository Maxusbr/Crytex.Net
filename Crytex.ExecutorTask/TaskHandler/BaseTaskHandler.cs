﻿using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    public abstract class BaseTaskHandler
    {
        public BaseTask TaskEntity { get; protected set; }
        public string Host { get; set; }

        public event EventHandler<BaseTask> ProcessingStarted;
        public event EventHandler<TaskExecutionResult> ProcessingFinished;

        protected abstract TaskExecutionResult ExecuteLogic();

        protected BaseTaskHandler(BaseTask task, string hostName)
        {
            this.TaskEntity = task;
            this.Host = hostName;
        }

        public TaskExecutionResult Execute()
        {
            this.ProcessingStarted.Invoke(this, this.TaskEntity);
            var taskResult = this.ExecuteLogic();
            taskResult.TaskEntity = this.TaskEntity;
            this.ProcessingFinished(this, taskResult);

            return taskResult;
        }
    }
}
