using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using Crytex.Model.Models.Notifications;
using Crytex.Notification;
using Crytex.Notification.Models;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerManager : ITaskHandlerManager
    {
        private ITaskVmService _taskService;
        private TaskHandlerFactory _handlerFactory = new TaskHandlerFactory();
        private IDictionary<Type, Action<int, StatusTask, string>> _updateStatusActionDict;
        private IUserVmService _userVmService;
        private INotificationManager _notificationManager;

        public TaskHandlerManager(ITaskVmService taskService, IUserVmService userVmService, INotificationManager notificationManager)
        {
            this._taskService = taskService;
            this._userVmService = userVmService;
            this._notificationManager = notificationManager;
            this._updateStatusActionDict = new Dictionary<Type, Action<int, StatusTask, string>>
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
            TaskEndNotify taskEndNotify = new TaskEndNotify
            {
                UserId = e.TaskEntity.UserId,
                TaskId = e.TaskEntity.Id,
                TypeError = TypeError.Unknown,
                TypeNotify = TypeNotify.EndTask,
                Success = e.Success,
                Error = e.ErrorMessage
            };
            if (e.Success)
            {
                this._updateStatusActionDict[taskType].Invoke(taskEntity.Id, StatusTask.End, null);
                this._notificationManager.SendToUserNotification(taskEndNotify.UserId, taskEndNotify);
            }
            else
            {
                this._updateStatusActionDict[taskType].Invoke(taskEntity.Id, StatusTask.EndWithErrors, e.ErrorMessage);
                this._notificationManager.SendToUserNotification(taskEndNotify.UserId, taskEndNotify);
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
                    UserId = task.UserId,
                    VurtualizationType = task.Virtualization
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
            this._updateStatusActionDict[task.GetType()].Invoke(task.Id, StatusTask.Processing, null);
        }

        private void UpdateCreateTaskStatusDelegate(int id, StatusTask status, string message)
        {
            this._taskService.UpdateTaskStatus<CreateVmTask>(id, status, message);
        }

        private void UpdateUpdateTaskkStatusDelegate(int id, StatusTask status, string message)
        {
            this._taskService.UpdateTaskStatus<UpdateVmTask>(id, status, message);
        }

        private void UpdateStandartTaskStatusDelegate(int id, StatusTask status, string message)
        {
            this._taskService.UpdateTaskStatus<StandartVmTask>(id, status, message);
        }
    }
}
