using Crytex.ExecutorTask;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crytex.Background.Tasks
{
    [DisallowConcurrentExecution]
    class TaskExecutorUpdateJob : IJob
    {
        private ITaskManager _taskManager;

        public TaskExecutorUpdateJob(ITaskManager taskManager)
        {
            this._taskManager = taskManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("It's task executor update job!");
            this._taskManager.UpdateTaskQueues();
        }
    }
}
