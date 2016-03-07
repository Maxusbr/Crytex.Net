using System;
using Quartz;
using Crytex.Service.IService;
using Crytex.Model.Models;
using Crytex.Background.Config;

namespace Crytex.Background.Tasks.SubscriptionVm
{
    [DisallowConcurrentExecution]
    public class SnapshotVmJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly ISnapshotVmService _snapshotVmService;
        private readonly ITaskV2Service _taskService;

        public SnapshotVmJob(ISnapshotVmService snapshotVmService, ITaskV2Service taskService, IBackgroundConfig config)
        {
            this._snapshotVmService = snapshotVmService;
            this._taskService = taskService;
            this._config = config;
        }
        public void Execute(IJobExecutionContext context)
        {
            var allSnaps = this._snapshotVmService.GetAllActive();
            var snapshotStoringDaysPeriod = this._config.GetSnapshotStoringDaysPeriod();
            foreach(var snapshot in allSnaps)
            {
                // Create delete task if snapshot is outdated
                if((DateTime.UtcNow.Day - snapshot.Date.Day) > snapshotStoringDaysPeriod)
                {
                    _snapshotVmService.PrepareSnapshotForDeletion(snapshot.Id, false);
                }
            }
        }
    }
}
