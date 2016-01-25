using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using Crytex.Model.Models.Notifications;
using Crytex.Notification;
using Crytex.Notification.Models;
using System.Linq;
using Crytex.Core;
using Crytex.Model.Enums;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerManager : ITaskHandlerManager
    {
        private ITaskV2Service _taskService;
        private TaskHandlerFactory _handlerFactory;
        private IUserVmService _userVmService;
        private INotificationManager _notificationManager;
        private IVmWareVCenterService _vmWareVCenterService;
        private IHyperVHostService _vmHyperVHostCenterService;
        private IVmBackupService _vmBackupService;
        private ITriggerService _triggerService;
        private readonly ISnapshotVmService _snapshotVmService;

        public TaskHandlerManager(ITaskV2Service taskService, IUserVmService userVmService,
            INotificationManager notificationManager, IVmWareVCenterService vmWareVCenterService,
            IOperatingSystemsService operatingSystemService, IHyperVHostService vmHyperVHostCenterService,
            IVmBackupService vmBackupService, ITriggerService triggerService, ISnapshotVmService snapshotVmService)
        {
            this._handlerFactory = new TaskHandlerFactory(operatingSystemService, snapshotVmService);
            this._taskService = taskService;
            this._userVmService = userVmService;
            this._notificationManager = notificationManager;
            this._vmWareVCenterService = vmWareVCenterService;
            this._vmHyperVHostCenterService = vmHyperVHostCenterService;
            this._vmBackupService = vmBackupService;
            this._triggerService = triggerService;
            this._snapshotVmService = snapshotVmService;
        }

        public IEnumerable<ITaskHandler> GetTaskHandlers(TypeVirtualization virtualizationType)
        {
            var tasks = this._taskService.GetPendingTasks(virtualizationType);

            var taskHandlers = this.GetTaskHandlerList(tasks, virtualizationType);

            return taskHandlers;
        }

        private List<ITaskHandler> GetTaskHandlerList(IEnumerable<TaskV2> tasks, TypeVirtualization virtualizationType)
        {
            var handlerList = new List<ITaskHandler>();
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

                handlerList.Add(handler);
                }

            return handlerList;
            }

        private VmWareVCenter GetVmWareVCenterForTask(TaskV2 task)
        {
            var vCenter = this._vmWareVCenterService.GetAllVCenters().First();
            return vCenter;
        }

        private HyperVHost GetHyperVHostForTask(TaskV2 task)
        {
            var hyperVHost = this._vmHyperVHostCenterService.GetAllHyperVHosts().First();
            return hyperVHost;
        }

        private void ProcessingFinishedEventHandler(object sender, TaskExecutionResult execResult)
        {
            var taskEntity = ((ITaskHandler)sender).TaskEntity;
            TaskEndNotify taskEndNotify = new TaskEndNotify
            {
                UserId = execResult.TaskEntity.UserId,
                Task = execResult.TaskEntity,
                TypeError = TypeError.Unknown,
                TypeNotify = TypeNotify.EndTask,

                Success = execResult.Success,
                Error = execResult.ErrorMessage
            };

            var finishDate = DateTime.UtcNow;
           
            if (execResult.Success)
            {
                this.UpdateTaskStatus(taskEntity.Id, StatusTask.End, finishDate, null);

                if (taskEntity.TypeTask == TypeTask.CreateVm)
                {

                    var taskOptions = execResult.TaskEntity.GetOptions<CreateVmOptions>();
                    var createTaskExecResult = (CreateVmTaskExecutionResult)execResult;

                    var vmId = createTaskExecResult.MachineGuid;

                    var newVm = new UserVm
                    {

                        Id = vmId,
                        CoreCount = taskOptions.Cpu,
                        HardDriveSize = taskOptions.Hdd,
                        Name = taskOptions.Name,
                        RamCount = taskOptions.Ram,
                        OperatingSystemId = taskOptions.OperatingSystemId,
                        Status = StatusVM.Enable,
                        CreateDate = DateTime.UtcNow,
                        UserId = execResult.TaskEntity.UserId,
                        VirtualizationType = execResult.TaskEntity.Virtualization,
                        OperatingSystemPassword = createTaskExecResult.GuestOsPassword,
                    };

                    switch (taskEntity.Virtualization)
                    {
                        case TypeVirtualization.HyperV:

                            newVm.HyperVHostId = execResult.VirtualizationServerEnitityId;
                            break;
                        case TypeVirtualization.VmWare:

                            newVm.VmWareCenterId = execResult.VirtualizationServerEnitityId;
                            break;
                    }

                    taskEntity.ResourceId = createTaskExecResult.MachineGuid;
                    this._userVmService.UpdateVm(newVm);

                    if (createTaskExecResult.IpAddresses != null)
                    {
                        this._userVmService.AddIpAddressesToVm(vmId, createTaskExecResult.IpAddresses);
                    }

                    this._taskService.UpdateTask(taskEntity);

                    var userVm = this._userVmService.GetVmById(createTaskExecResult.TaskEntity.GetOptions<CreateVmOptions>().UserVmId);
                    this._notificationManager.SendNewVmCreationEndEmail(taskEntity.UserId, userVm.Name, userVm.OperatingSystem.DefaultAdminName, userVm.OperatingSystemPassword);
                }
                else if (taskEntity.TypeTask == TypeTask.UpdateVm)
                {

                    var taskOptions = execResult.TaskEntity.GetOptions<UpdateVmOptions>();
                    this._userVmService.UpdateVm(taskOptions.VmId, taskOptions.Cpu, taskOptions.Hdd, taskOptions.Ram);
                }
                else if (taskEntity.TypeTask == TypeTask.ChangeStatus)
                {

                    var taskOptions = execResult.TaskEntity.GetOptions<ChangeStatusOptions>();
                    this._userVmService.UpdateVmStatus(taskOptions.VmId, taskOptions.TypeChangeStatus);
                }
                else if (taskEntity.TypeTask == TypeTask.Backup)
                {
                    var backupExecResult = (BackupTaskExecutionResult)execResult;

                    this._vmBackupService.UpdateBackupStatus(backupExecResult.BackupGuid, VmBackupStatus.Active);
                }
                else if (taskEntity.TypeTask == TypeTask.DeleteBackup)
                {
                    var backupExecResult = (BackupTaskExecutionResult)execResult;

                    this._vmBackupService.DeleteBackupDbEntity(backupExecResult.BackupGuid);
                }
                else if (taskEntity.TypeTask == TypeTask.RemoveVm)
                {
                    var options = taskEntity.GetOptions<RemoveVmOptions>();
                    this._userVmService.MarkAsDeleted(options.VmId);
                }
                else if (taskEntity.TypeTask == TypeTask.CreateSnapshot)
                {
                    var createSnapshotResult = (CreateSnapshotExecutionResult)execResult;
                    this._snapshotVmService.ActivateNewlyCreatedSnapshot(createSnapshotResult.SnapshotGuid);
                }
                else if(taskEntity.TypeTask == TypeTask.DeleteSnapshot)
                {
                    var taskOptions = taskEntity.GetOptions<DeleteSnapshotOptions>();
                    this._snapshotVmService.DeleteSnapshot(taskOptions.SnapshotId, taskOptions.DeleteWithChildrens);
                }
                else if(taskEntity.TypeTask == TypeTask.LoadSnapshot)
                {
                    var taskOptions = taskEntity.GetOptions<LoadSnapshotOptions>();
                    this._snapshotVmService.SetLoadedSnapshotActive(taskOptions.SnapshotId);
                }
            }
            else
            {
                this.UpdateTaskStatus(taskEntity.Id, StatusTask.EndWithErrors, finishDate, execResult.ErrorMessage);
            }

            try
            {
                this._notificationManager.SendToUserNotification(taskEndNotify.UserId, taskEndNotify);
            }
            catch (Exception ex)
            {
                LoggerCrytex.Logger.Error("Ошибка отправки сообщения " + ex);
            }

            var standartTigger = this._triggerService.GetUserTrigger(taskEntity.UserId, TriggerType.EndTask, Convert.ToDouble(taskEntity.TypeTask));
            if (standartTigger != null)
            {
                _notificationManager.SendEmailUserByTask(taskEntity.UserId, taskEntity.TypeTask);
            }   
        }

        private void ProcessingStartedEventHandler(object sender, TaskV2 task)
        {
            var startTime = DateTime.UtcNow;
            this.UpdateTaskStatus(task.Id, StatusTask.Start, startTime, null);
        }

        private void UpdateTaskStatus(Guid id, StatusTask status, DateTime date, string message)
        {
            this._taskService.UpdateTaskStatus(id, status, date, message);
        }
    }
}
