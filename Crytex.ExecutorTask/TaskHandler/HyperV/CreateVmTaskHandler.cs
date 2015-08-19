using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class CreateVmTaskHandler : ITaskHandler
    {
        private CreateVmTask _taskEntity;

        public CreateVmTaskHandler(CreateVmTask task)
        {
            this._taskEntity = task;
        }

        public TaskExecutionResult Execute()
        {
            this.ProcessingStarted.Invoke(this, this.TaskEntity);
            Console.WriteLine("Fake task handler");
            return new TaskExecutionResult();
        }

        public BaseTask TaskEntity
        {
            get { return this._taskEntity; }
        }

        public event EventHandler<BaseTask> ProcessingStarted;
    }
}
