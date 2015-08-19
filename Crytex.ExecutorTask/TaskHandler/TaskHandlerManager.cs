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
        private IDictionary<Type, Action<int, StatusTask>> _updateActionDict;

        public TaskHandlerManager(ITaskVmService taskService)
        {
            this._taskService = taskService;
            this._updateActionDict = new Dictionary<Type, Action<int, StatusTask>>
            {
                {typeof(CreateVmTask), this.UpdateCreateTaskStatusDelegate},
                {typeof(UpdateVmTask), this.UpdateUpdateTaskkStatusDelegate},
                {typeof(StandartVmTask), this.UpdateStandartTaskStatusDelegate}
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
                handler.ProcessingFinished += this.ProcessingFinishedEventHandler;
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

        private void ProcessingFinishedEventHandler(object sender, TaskExecutionResult e)
        {
            var taskEntity = ((ITaskHandler)sender).TaskEntity;
            this._updateActionDict[taskEntity.GetType()].Invoke(taskEntity.Id, StatusTask.End);
        }

        private void ProcessingStartedEventHandler(object sender, BaseTask task)
        {
            this._updateActionDict[task.GetType()].Invoke(task.Id, StatusTask.Processing);
        }

        private void UpdateCreateTaskStatusDelegate(int id, StatusTask status)
        {
            this._taskService.UpdateTaskStatus<CreateVmTask>(id, status);
        }
        private void UpdateUpdateTaskkStatusDelegate(int id, StatusTask status)
        {
            this._taskService.UpdateTaskStatus<UpdateVmTask>(id, status);
        }
        private void UpdateStandartTaskStatusDelegate(int id, StatusTask status)
        {
            this._taskService.UpdateTaskStatus<StandartVmTask>(id, status);
        }
    }
}
