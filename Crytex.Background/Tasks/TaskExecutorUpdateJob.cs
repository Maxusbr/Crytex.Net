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
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    class TaskExecutorUpdateJob : IJob
    {
        private ITaskQueuePoolManager _taskManager;

        public TaskExecutorUpdateJob(ITaskQueuePoolManager taskManager)
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
