using Project.Model.Models;
using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerManager : ITaskHandlerManager
    {
        private ITaskVmService _taskService;
        private TaskHandlerFactory _handlerFactory = new TaskHandlerFactory();

        private IDictionary<Type, Action<int>> _updateActionDict;
        public TaskHandlerManager(ITaskVmService taskService)
        {
            this._taskService = taskService;
            this._updateActionDict = new Dictionary<Type, Action<int>>
            {
                {typeof(CreateVmTask), this.UpdateCreateTaskStatusDelegate}
            };
        }

        public PendingTaskHandlerBox GetTaskHandlers()
        {
            var createTasks = this._taskService.GetPendingCreateTasks();
            var updateTasks = this._taskService.GetPendingUpdateTasks();
            var standartTasks = this._taskService.GetPendingStandartTasks();

            var wmWareTaskHandlers = new List<ITaskHandler>();
            var hyperVTaskHandlers = new List<ITaskHandler>();
            this.PopulateTaskLists(createTasks, wmWareTaskHandlers, hyperVTaskHandlers);
            this.PopulateTaskLists(updateTasks, wmWareTaskHandlers, hyperVTaskHandlers);
            this.PopulateTaskLists(standartTasks, wmWareTaskHandlers, hyperVTaskHandlers);

            return new PendingTaskHandlerBox
            {
                HyperVHandlers = hyperVTaskHandlers,
                VmWareHandlers = wmWareTaskHandlers
            };
        }

        private void PopulateTaskLists<T>(IEnumerable<T> tasks, IList<ITaskHandler> wmWareTaskHandlers,
            IList<ITaskHandler> hyperVTaskHandlers) where T : BaseTask
        {
            foreach (var task in tasks)
            {
                
                var handler = this._handlerFactory.GetHandler(task);
                handler.ProcessingStarted += this.ProcessingStartedEventHandler;
                switch (task.Virtualization)
                {
                    case TypeVirtualization.HyperV:
                        hyperVTaskHandlers.Add(handler);
                        break;
                    case TypeVirtualization.WmWare:
                        wmWareTaskHandlers.Add(handler);
                        break;
                }
            }
        }

        private void ProcessingStartedEventHandler(object sender, BaseTask task)
        {
            this.UpdateTaskStatus(task);
        }

        public void UpdateTaskStatus(BaseTask task)
        {
            this._updateActionDict[task.GetType()].Invoke(task.Id);
        }

        private void UpdateCreateTaskStatusDelegate(int id)
        {
            this._taskService.UpdateTaskStatus<CreateVmTask>(id, StatusTask.Processing);
        }
    }
}
