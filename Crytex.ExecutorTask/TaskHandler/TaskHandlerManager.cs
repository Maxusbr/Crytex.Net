using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using Crytex.Model.Models.Notifications;
using Crytex.Notification;
using Crytex.Notification.Models;
using System.Linq;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerManager : ITaskHandlerManager
    {
        private ITaskV2Service _taskService;
        private TaskHandlerFactory _handlerFactory;
        private IUserVmService _userVmService;
        private INotificationManager _notificationManager;
        private IVmWareVCenterService _vmWareVCenterService;

        public TaskHandlerManager(ITaskV2Service taskService, IUserVmService userVmService,
            INotificationManager notificationManager, IVmWareVCenterService vmWareVCenterService,
            IServerTemplateService serverTemplateService)
        {
            this._handlerFactory = new TaskHandlerFactory(serverTemplateService);
            this._taskService = taskService;
            this._userVmService = userVmService;
            this._notificationManager = notificationManager;
            this._vmWareVCenterService = vmWareVCenterService;
        }

        public PendingTaskHandlerBox GetTaskHandlers()
        {
            var tasks = this._taskService.GetPendingTasks();

            var wmWareTaskHandlers = new List<ITaskHandler>();
            var hyperVTaskHandlers = new List<ITaskHandler>();
            this.PopulateTaskLists(tasks, wmWareTaskHandlers, hyperVTaskHandlers);

            return new PendingTaskHandlerBox
            {
                HyperVHandlers = hyperVTaskHandlers,
                VmWareHandlers = wmWareTaskHandlers
            };
        }

        private void PopulateTaskLists(IEnumerable<TaskV2> tasks, IList<ITaskHandler> wmWareTaskHandlers,
            IList<ITaskHandler> hyperVTaskHandlers)
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
                    case TypeVirtualization.VmWare:
                        var vmWareVCenter = this.GetVmWareVCenterForTask(task);
                        handler = this._handlerFactory.GetVmWareHandler(task, vmWareVCenter);
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
                    case TypeVirtualization.VmWare:
                        wmWareTaskHandlers.Add(handler);
                        break;
                }
            }
        }

        private VmWareVCenter GetVmWareVCenterForTask(TaskV2 task)
        {
            var vCenter = this._vmWareVCenterService.GetAllVCenters().First();

            return vCenter;
        }

        private HyperVHost GetHyperVHostForTask(TaskV2 task)
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

            var finishDate = DateTime.UtcNow;
            if (e.Success)
            {
                this.UpdateTaskStatus(taskEntity.Id, StatusTask.End, finishDate, null);
                this._notificationManager.SendToUserNotification(taskEndNotify.UserId, taskEndNotify);
            }
            else
            {
                this.UpdateTaskStatus(taskEntity.Id, StatusTask.EndWithErrors, finishDate, e.ErrorMessage);
                this._notificationManager.SendToUserNotification(taskEndNotify.UserId, taskEndNotify);
            }

            if (taskType == typeof(CreateVmTask))
            {
                var taskOptions = e.TaskEntity.GetOptions<CreateVmOptions>();
                var newVm = new UserVm
                {
                    Id = e.MachineGuid,
                    CoreCount = taskOptions.Cpu,
                    HardDriveSize = taskOptions.Hdd,
                    Name = taskOptions.Name,
                    RamCount = taskOptions.Ram,
                    ServerTemplateId = taskOptions.ServerTemplateId,
                    Status = StatusVM.Enable,
                    UserId = e.TaskEntity.UserId,
                    VurtualizationType = e.TaskEntity.Virtualization
                };

                switch (taskEntity.Virtualization)
                {
                    case TypeVirtualization.HyperV:
                        newVm.VmWareCenterId = e.VirtualizationServerEnitityId;
                        break;
                    case TypeVirtualization.VmWare:
                        newVm.HyperVHostId = e.VirtualizationServerEnitityId;
                        break;
                }

                this._userVmService.CreateVm(newVm);
            }
            if (taskType == typeof(UpdateVmTask))
            {
                var taskOptions = e.TaskEntity.GetOptions<UpdateVmOptions>();
                this._userVmService.UpdateVm(taskOptions.VmId, taskOptions.Cpu, taskOptions.Hdd, taskOptions.Ram);
            }
        }

        private void ProcessingStartedEventHandler(object sender, TaskV2 task)
        {
            var startTime = DateTime.UtcNow;
            this.UpdateTaskStatus(task.Id, StatusTask.Processing, startTime, null);
        }

        private void UpdateTaskStatus(Guid id, StatusTask status, DateTime date, string message)
        {
            this._taskService.UpdateTaskStatus(id, status, date, message);
        }
    }
}
