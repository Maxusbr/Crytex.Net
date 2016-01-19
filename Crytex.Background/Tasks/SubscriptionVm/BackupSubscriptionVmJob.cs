using System.Linq;
using Quartz;
using Crytex.Service.IService;
using Crytex.Model.Models.Biling;
using System;
using Crytex.Model.Models;

namespace Crytex.Background.Tasks.SubscriptionVm
{
    [DisallowConcurrentExecution]
    public class BackupSubscriptionVmJob : IJob
    {
        private readonly ISubscriptionVmService _subscriptionService;
        private readonly ITaskV2Service _taskService;
        private readonly IVmBackupService _vmBackupService;

        public BackupSubscriptionVmJob(ISubscriptionVmService subscriptionService, IVmBackupService vmBackupService,
            ITaskV2Service taskService)
        {
            this._subscriptionService = subscriptionService;
            this._vmBackupService = vmBackupService;
            this._taskService = taskService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var subs = this._subscriptionService.GetAllByStatus(SubscriptionVmStatus.Active);
            foreach(var sub in subs)
            {
                var subVmBackups = this._vmBackupService.GetByVmId(sub.UserVm.Id);

                // Create vm todays backup if not exist
                var todaysBackupExist = subVmBackups.Any(b => b.DateCreated.Day == DateTime.UtcNow.Day);
                if (!todaysBackupExist)
                {
                    // create backup task
                    var backupTask = new TaskV2
                    {
                        ResourceId = sub.UserVm.Id,
                        ResourceType = ResourceType.Vm,
                        TypeTask = TypeTask.Backup,
                        Virtualization = sub.UserVm.VirtualizationType,
                        UserId = sub.UserId
                    };
                    var backupTaskOptions = new BackupOptions
                    {
                        BackupName = "Automatic backup",
                        VmId = sub.UserVm.Id
                    };

                    this._taskService.CreateTask(backupTask, backupTaskOptions);
                }

                // Delete outdated backups
                var outdatedBackups = subVmBackups.Where(b => DateTime.UtcNow.Day - b.DateCreated.Day > sub.DailyBackupStorePeriodDays
                    && b.Status == VmBackupStatus.Active);
                foreach(var backup in outdatedBackups)
                {
                    // delete backup
                    var deleteBackupTask = new TaskV2
                    {
                        ResourceId = sub.UserVm.Id,
                        ResourceType = ResourceType.Vm,
                        TypeTask = TypeTask.DeleteBackup,
                        Virtualization = sub.UserVm.VirtualizationType,
                        UserId = sub.UserId
                    };
                    var deleteBackupTaskOptions = new DeleteBackupOptions
                    {
                        VmBackupId = backup.Id
                    };

                    this._taskService.CreateTask(deleteBackupTask, deleteBackupTaskOptions);
                }
            }
        }
    }
}
