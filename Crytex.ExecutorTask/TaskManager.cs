using Crytex.ExecutorTask.TaskHandler;
using Project.Model.Models;
using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask
{
    public class TaskManager
    {
        private ITaskHandlerManager _taskHandlerManager;
        private TaskQueueManager _vmWareTaskQueueManager = new TaskQueueManager();
        private TaskQueueManager _hyperVTaskQueueManager = new TaskQueueManager();

        public TaskManager(ITaskHandlerManager taskHandlerManager)
        {
            this._taskHandlerManager = taskHandlerManager;
        }

        public void Run()
        {
            var thread = new System.Threading.Thread(this.RunInner);
            thread.Start();

            this._hyperVTaskQueueManager.ExecuteAsync();
            this._vmWareTaskQueueManager.ExecuteAsync();
        }

        private void RunInner()
        {
            while (true)
            {
                var handlers = this._taskHandlerManager.GetTaskHandlers();

                this.AddHandlersToQueue(handlers.HyperVHandlers, this._hyperVTaskQueueManager);
                this.AddHandlersToQueue(handlers.VmWareHandlers, this._vmWareTaskQueueManager);

                Thread.Sleep(EXECUTE_TIMEOUT);
            }
        }

        private void AddHandlersToQueue(IEnumerable<ITaskHandler> handlers, TaskQueueManager queueManager)
        {
            foreach (var handler in handlers)
            {
                queueManager.AddToQueue(handler);
            }
        }

        private const int EXECUTE_TIMEOUT = 1000;
    }
}
