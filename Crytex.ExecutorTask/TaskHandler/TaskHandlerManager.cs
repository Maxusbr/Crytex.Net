using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerManager : ITaskHandlerManager
    {
        private ITaskVmService _taskService;
        private TaskHandlerFactory _handlerFactory = new TaskHandlerFactory();
        private IDictionary<Type, Action<int, StatusTask>> _updateStatusActionDict;
        private IUserVmService _userVmService;

        public TaskHandlerManager(ITaskVmService taskService, IUserVmService userVmService)
        {
            this._taskService = taskService;
            this._userVmService = userVmService;
            this._updateStatusActionDict = new Dictionary<Type, Action<int, StatusTask>>
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
                ITaskHandler handler = null;
                switch (task.Virtualization)
                {
                    case TypeVirtualization.HyperV:
                        var hyperVHost = this.GetHyperVHostForTask(task);
                        handler = this._handlerFactory.GetHyperVHandler(task, hyperVHost);
                        break;
                    case TypeVirtualization.WmWare:
                        var vmWareHost = this.GetVmWareHostForTask(task);
                        handler = this._handlerFactory.GetVmWareHandler(task, vmWareHost);
                        break;
                    default:
                        throw new ApplicationException(string.Format("Unknown virtualization type {0}", task.Virtualization));
                }
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

        private VmWareHost GetVmWareHostForTask(BaseTask task)
        {
            return new VmWareHost();
        }

        private HyperVHost GetHyperVHostForTask(BaseTask task)
        {
            return new HyperVHost();
        }

        private void ProcessingFinishedEventHandler(object sender, TaskExecutionResult e)
        {
            var taskEntity = ((ITaskHandler)sender).TaskEntity;
            var taskType = taskEntity.GetType();
            if (e.Success)
            {
                this._updateStatusActionDict[taskType].Invoke(taskEntity.Id, StatusTask.End);
            }
            else
            {
                this._updateStatusActionDict[taskType].Invoke(taskEntity.Id, StatusTask.EndWithErrors);
            }

            if (taskType == typeof(CreateVmTask))
            {
                var task = (CreateVmTask)taskEntity;
                var newVm = new UserVm
                {
                    Id = e.MachineGuid,
                    CoreCount = task.Cpu,
                    HardDriveSize = task.Hdd,
                    Name = task.Name,
                    RamCount = task.Ram,
                    ServerTemplateId = task.ServerTemplateId,
                    Status = StatusVM.Enable,
                    UserId = task.UserId
                };

                this._userVmService.CreateVm(newVm);
            }
            if (taskType == typeof(UpdateVmTask))
            {
                var task = (UpdateVmTask)taskEntity;
                this._userVmService.UpdateVm(task.VmId, task.Cpu, task.Hdd, task.Ram);
            }
        }

        private void ProcessingStartedEventHandler(object sender, BaseTask task)
        {
            this._updateStatusActionDict[task.GetType()].Invoke(task.Id, StatusTask.Processing);
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
