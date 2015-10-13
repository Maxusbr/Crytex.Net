﻿using Crytex.ExecutorTask;
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
        private ITaskManager _taskManager;

        public TaskExecutorUpdateJob(ITaskManager taskManager)
        {
            this._taskManager = taskManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            this._taskManager.UpdateTaskQueues();
            Console.WriteLine("It's task executor update job!");
        }
    }
}
