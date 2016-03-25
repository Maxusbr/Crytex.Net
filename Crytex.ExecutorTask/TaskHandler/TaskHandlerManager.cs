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
using Crytex.Model.Models.GameServers;

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
        private readonly IGameServerService _gameServerService;

        private IDictionary<TypeTask, Action<TaskV2, TaskExecutionResult>> _taskTypeSpecificProcessingFinishedActions;

        public TaskHandlerManager(ITaskV2Service taskService, IUserVmService userVmService,
            INotificationManager notificationManager, IVmWareVCenterService vmWareVCenterService,
            IOperatingSystemsService operatingSystemService, IHyperVHostService vmHyperVHostCenterService,
            IVmBackupService vmBackupService, ITriggerService triggerService, ISnapshotVmService snapshotVmService,
            IGameServerService gameServerService)
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
            _gameServerService = gameServerService;

            this._taskTypeSpecificProcessingFinishedActions = new Dictionary<TypeTask, Action<TaskV2, TaskExecutionResult>>
            {
                {TypeTask.CreateVm, this.CreateVmProcessingFinished},
                {TypeTask.UpdateVm, this.UpdateVmProcessingFinished},
                {TypeTask.ChangeStatus, this.ChangeStatusProcessingFinished},
                {TypeTask.Backup, this.BackupProcessingFinished},
                {TypeTask.DeleteBackup, this.DeleteBackupProcessingFinished},
                {TypeTask.RemoveVm, this.RemoveVmProcessingFinished},
                {TypeTask.CreateSnapshot, this.CreateSnapshotProcessingFinished},
                {TypeTask.DeleteSnapshot, this.DeleteSnapshotProcessingFinished},
                {TypeTask.LoadSnapshot, this.LoadSnapshotProcessingFinished},
                {TypeTask.CreateGameServer,  CreateGameServerProcessingFinished},
                {TypeTask.GameServerChangeStatus,  GameServerChangeStatusProcessingFinished}
            };
        }

        public IEnumerable<ITaskHandler> GetTaskHandlers(IEnumerable<TaskV2> tasks)
        {
            var taskHandlers = this.GetTaskHandlerList(tasks);

            return taskHandlers;
        }

        private List<ITaskHandler> GetTaskHandlerList(IEnumerable<TaskV2> tasks)
        {
            var handlerList = new List<ITaskHandler>();
            foreach (var task in tasks)
            {
                ITaskHandler handler = null;
                if (IsGameTask(task))
                {
                    var gameServer = GetGameServerForTask(task);
                    handler = _handlerFactory.GetGameTaskHandler(task, gameServer);
                }
                else
                {
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
                }
                
                handler.ProcessingStarted += this.ProcessingStartedEventHandler;
                handler.ProcessingFinished += this.ProcessingFinishedEventHandler;

                handlerList.Add(handler);
                }

            return handlerList;
        }

        private GameServer GetGameServerForTask(TaskV2 task)
        {
            var gameHost = _gameServerService.GetById((task.GetOptions<BaseGameServerOptions>()).GameServerId);

            return gameHost;
        }

        private bool IsGameTask(TaskV2 task)
        {
            if (task.TypeTask == TypeTask.CreateGameServer ||
                task.TypeTask == TypeTask.GameServerChangeStatus ||
                task.TypeTask == TypeTask.DeleteGameServer)
                return true;

            return false;
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

            var finishDate = DateTime.UtcNow;
            if (execResult.Success)
            {
                this.UpdateTaskStatus(taskEntity.Id, StatusTask.End, finishDate, null);

                if (this._taskTypeSpecificProcessingFinishedActions.ContainsKey(taskEntity.TypeTask))
                {
                    this._taskTypeSpecificProcessingFinishedActions[taskEntity.TypeTask].Invoke(taskEntity, execResult);
                }
            }
            else
            {
                this.UpdateTaskStatus(taskEntity.Id, StatusTask.EndWithErrors, finishDate, execResult.ErrorMessage);
            }

            try
            {
                TaskEndNotify taskEndNotify = new TaskEndNotify
                {
                    UserId = execResult.TaskEntity.UserId,
                    Task = execResult.TaskEntity,
                    TypeError = TypeError.Unknown,
                    TypeNotify = TypeNotify.EndTask,

                    Success = execResult.Success,
                    Error = execResult.ErrorMessage
                };
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

        #region Task type specific ProcessingFinished actions
        private void LoadSnapshotProcessingFinished(TaskV2 taskEntity, TaskExecutionResult execResult)
        {
            var taskOptions = taskEntity.GetOptions<LoadSnapshotOptions>();
            this._snapshotVmService.SetLoadedSnapshotActive(taskOptions.SnapshotId);
        }

        private void DeleteSnapshotProcessingFinished(TaskV2 taskEntity, TaskExecutionResult execResult)
        {
            var taskOptions = taskEntity.GetOptions<DeleteSnapshotOptions>();
            this._snapshotVmService.DeleteSnapshot(taskOptions.SnapshotId, taskOptions.DeleteWithChildrens);
        }

        private void CreateSnapshotProcessingFinished(TaskV2 task, TaskExecutionResult execResult)
        {
            var createSnapshotResult = (CreateSnapshotExecutionResult)execResult;
            this._snapshotVmService.ActivateNewlyCreatedSnapshot(createSnapshotResult.SnapshotGuid);
        }

        private void RemoveVmProcessingFinished(TaskV2 taskEntity, TaskExecutionResult execResult)
        {
            var options = taskEntity.GetOptions<RemoveVmOptions>();
            this._userVmService.MarkAsDeleted(options.VmId);
        }

        private void DeleteBackupProcessingFinished(TaskV2 task, TaskExecutionResult execResult)
        {
            var backupExecResult = (BackupTaskExecutionResult)execResult;

            this._vmBackupService.DeleteBackupDbEntity(backupExecResult.BackupGuid);
        }

        private void BackupProcessingFinished(TaskV2 task, TaskExecutionResult execResult)
        {
            var backupExecResult = (BackupTaskExecutionResult)execResult;

            this._vmBackupService.UpdateBackupStatus(backupExecResult.BackupGuid, VmBackupStatus.Active);
        }

        private void ChangeStatusProcessingFinished(TaskV2 task, TaskExecutionResult execResult)
        {
            var taskOptions = execResult.TaskEntity.GetOptions<ChangeStatusOptions>();
            this._userVmService.UpdateVmStatus(taskOptions.VmId, taskOptions.TypeChangeStatus);
        }

        private void UpdateVmProcessingFinished(TaskV2 task, TaskExecutionResult execResult)
        {
            var taskOptions = execResult.TaskEntity.GetOptions<UpdateVmOptions>();
            this._userVmService.UpdateVm(taskOptions.VmId, taskOptions.Cpu, taskOptions.HddGB, taskOptions.Ram);
        }

        private void CreateVmProcessingFinished(TaskV2 taskEntity, TaskExecutionResult execResult)
        {
            var taskOptions = execResult.TaskEntity.GetOptions<CreateVmOptions>();
            var createTaskExecResult = (CreateVmTaskExecutionResult)execResult;

            var vmId = createTaskExecResult.MachineGuid;

            var newVm = new UserVm
            {

                Id = vmId,
                CoreCount = taskOptions.Cpu,
                HardDriveSize = taskOptions.HddGB,
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


        private void CreateGameServerProcessingFinished(TaskV2 task, TaskExecutionResult executionResult)
        {
            var createServerExecResult = (CreateGameServerTaskExecutionResult) executionResult;
            var taskOptions = task.GetOptions<CreateGameServerOptions>();

            _gameServerService.UpdatePassword(taskOptions.GameServerId, createServerExecResult.ServerNewPassword);
            _gameServerService.UpdateServerState(taskOptions.GameServerId, GameServerState.Disable);
        }


        private void GameServerChangeStatusProcessingFinished(TaskV2 task, TaskExecutionResult res)
        {
            var taskOptions = task.GetOptions<ChangeGameServerStatusOptions>();
            switch (taskOptions.TypeChangeStatus)
            {
                case TypeChangeStatus.Start:
                    _gameServerService.UpdateServerState(taskOptions.GameServerId, GameServerState.Enable);
                    break;
                case TypeChangeStatus.Stop:
                    _gameServerService.UpdateServerState(taskOptions.GameServerId, GameServerState.Disable);
                    break;
            }
        }


        #endregion

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
