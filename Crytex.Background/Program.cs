﻿namespace Crytex.Background
{
    using Project.Core;
    using Scheduler;
    using Tasks;

    public static class Program
    {
        public static void Main(string[] args)
        {
            LoggerCrytex.SetSource(SourceLog.Background);
            UnityConfig.Configure();
            var scheduler = UnityConfig.Resolve<ISchedulerJobs>();

            scheduler.StartScheduler();

            scheduler.ScheduleJob<BillingJob>("billing", "*/3 * * * * ?");
            scheduler.ScheduleJob<MonitoringJob>("monitoring", "*/5 * * * * ?");


            LoggerCrytex.Logger.Info("Hello from Background");
        }

         
    }
}