using Crytex.ExecutorTask.TaskHandler;
using Crytex.Service.IService;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Threading;

namespace Crytex.ExecutorTask
{
    public class TaskManager : ITaskManager
    {
        private ITaskHandlerManager _hyperVTaskHandlerManager;
        private ITaskHandlerManager _vmWareTaskHandlerManager;
        private TaskQueueManager _vmWareTaskQueueManager = new TaskQueueManager();
        private TaskQueueManager _hyperVTaskQueueManager = new TaskQueueManager();
        private readonly ITaskV2Service _taskService;

        public TaskManager(IUnityContainer unityContainer, ITaskV2Service taskService)
        {
            this._hyperVTaskHandlerManager = unityContainer.Resolve<ITaskHandlerManager>();
            this._vmWareTaskHandlerManager = unityContainer.Resolve<ITaskHandlerManager>();
            this._taskService = taskService;
        }

        public void RunTasks()
        {
            this._hyperVTaskQueueManager.ExecuteAsync();
            this._vmWareTaskQueueManager.ExecuteAsync();
        }

        public void UpdateTaskQueues()
        {
            var hyperVHandlers = this._hyperVTaskHandlerManager.GetTaskHandlers(Model.Models.TypeVirtualization.HyperV);
            var vmWareHandlers = this._vmWareTaskHandlerManager.GetTaskHandlers(Model.Models.TypeVirtualization.VmWare);

            this.AddHandlersToQueue(hyperVHandlers, this._hyperVTaskQueueManager);
            this.AddHandlersToQueue(vmWareHandlers, this._vmWareTaskQueueManager);
        }

        private void AddHandlersToQueue(IEnumerable<ITaskHandler> handlers, TaskQueueManager queueManager)
        {
            foreach (var handler in handlers)
            {
                this._taskService.UpdateTaskStatus(handler.TaskEntity.Id, Model.Models.StatusTask.Queued);
                queueManager.AddToQueue(handler);
            }
        }
    }
}
