using System.Linq;
using Quartz;
using Crytex.Service.IService;
using Crytex.Model.Models.Biling;
using System;

namespace Crytex.Background.Tasks.SubscriptionVm
{
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
            var subs = this._subscriptionService.GetAllByStatus(SubscriptionVmStatus.Active);
            foreach(var sub in subs)
            {
                var subVmBackups = this._vmBackupService.GetByVmId(sub.UserVm.Id);

                // Create vm todays backup if not exist
                var todaysBackupExist = subVmBackups.Any(b => b.DateCreated.Day == DateTime.UtcNow.Day);
                if (!todaysBackupExist)
                {
                    // TODO: create backup task
                }

                // Delete outdated backups
                var outdatedBackups = subVmBackups.Where(b => b.DateCreated.Day - DateTime.UtcNow.Day > sub.DailyBackupStorePeriodDays);
                foreach(var backup in outdatedBackups)
                {
                    // TODO: delete backup
                }
            }
        }
    }
}
