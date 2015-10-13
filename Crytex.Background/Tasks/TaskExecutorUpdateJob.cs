using Crytex.ExecutorTask;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Background.Tasks
{
    class TaskExecutorUpdateJob : IJob
    {
        public ITaskManager TaskManager { get; set; }

        public void Execute(IJobExecutionContext context)
        {
            this.TaskManager.UpdateTaskQueues();
            Console.WriteLine("It's task executor update job!");
        }
    }
}
