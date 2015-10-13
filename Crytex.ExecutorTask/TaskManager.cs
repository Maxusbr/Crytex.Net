using Crytex.ExecutorTask.TaskHandler;
using System.Collections.Generic;
using System.Threading;

namespace Crytex.ExecutorTask
{
    public class TaskManager : ITaskManager
    {
        private ITaskHandlerManager _taskHandlerManager;
        private TaskQueueManager _vmWareTaskQueueManager = new TaskQueueManager();
        private TaskQueueManager _hyperVTaskQueueManager = new TaskQueueManager();

        public TaskManager(ITaskHandlerManager taskHandlerManager)
        {
            this._taskHandlerManager = taskHandlerManager;
        }

        public void RunTasks()
        {
            this._hyperVTaskQueueManager.ExecuteAsync();
            this._vmWareTaskQueueManager.ExecuteAsync();
        }

        public void UpdateTaskQueues()
        {
            var handlers = this._taskHandlerManager.GetTaskHandlers();

            this.AddHandlersToQueue(handlers.HyperVHandlers, this._hyperVTaskQueueManager);
            this.AddHandlersToQueue(handlers.VmWareHandlers, this._vmWareTaskQueueManager);
        }

        private void AddHandlersToQueue(IEnumerable<ITaskHandler> handlers, TaskQueueManager queueManager)
        {
            foreach (var handler in handlers)
            {
                queueManager.AddToQueue(handler);
            }
        }
    }
}
