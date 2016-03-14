using System;
using Crytex.Model.Models;
using System.Threading;

namespace Crytex.ExecutorTask.TaskHandler
{
    class TestTaskHandler : ITaskHandler
    {
        public TaskV2 TaskEntity { get; set; }

        public event EventHandler<TaskExecutionResult> ProcessingFinished;
        public event EventHandler<TaskV2> ProcessingStarted;

        public TestTaskHandler(TaskV2 task)
        {
            this.TaskEntity = task;
        }

        public TaskExecutionResult Execute()
        {
            var result = new TaskExecutionResult
            {
                Success = true,
                TaskEntity = this.TaskEntity
            };
            this.ProcessingStarted.Invoke(this, this.TaskEntity);

            Console.WriteLine($"Test task handler. UserId = {this.TaskEntity.UserId}");
            Thread.Sleep(7000);

            this.ProcessingFinished.Invoke(this, result);

            return result;
        }
    }
}
