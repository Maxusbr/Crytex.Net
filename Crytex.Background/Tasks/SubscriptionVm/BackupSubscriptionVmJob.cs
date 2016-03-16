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
        private readonly IVmBackupService _vmBackupService;

        public BackupSubscriptionVmJob(ISubscriptionVmService subscriptionService, IVmBackupService vmBackupService)
        {
            this._subscriptionService = subscriptionService;
            this._vmBackupService = vmBackupService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var subs = this._subscriptionService.GetSubscriptionsByStatusAndType(SubscriptionVmStatus.Active);
            foreach(var sub in subs)
            {
                var subVmBackups = this._vmBackupService.GetByVmId(sub.UserVm.Id);

                // Create vm todays backup if not exist
                var todaysBackupExist = subVmBackups.Any(b => b.DateCreated.Day == DateTime.UtcNow.Day);
                if (!todaysBackupExist)
                {
                    _vmBackupService.Create(sub.Id, "Automatic backup");
                }

                // Delete outdated backups
                var outdatedBackups = subVmBackups.Where(b => DateTime.UtcNow.Day - b.DateCreated.Day > sub.DailyBackupStorePeriodDays
                    && b.Status == VmBackupStatus.Active);
                foreach(var backup in outdatedBackups)
                {
                    _vmBackupService.Delete(backup.Id);
                }
            }
        }
    }
}
