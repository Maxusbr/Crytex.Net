using Crytex.ExecutorTask;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Crytex.Background.Tasks
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    class TaskExecutorUpdateJob : IJob
    {
        private ITaskQueuePoolManager _taskManager;

        public TaskExecutorUpdateJob(IUnityContainer unityContainer, string executorName)
        {
            this._taskManager = unityContainer.Resolve<ITaskQueuePoolManager>(executorName);
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("It's task executor update job!");
            this._taskManager.UpdateTaskQueues();
        }
    }
}
