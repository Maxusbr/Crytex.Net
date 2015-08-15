namespace Crytex.Background
{
    using Scheduler;
    using Tasks;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var scheduler = UnityConfig.Resolve<ISchedulerJobs>();

            scheduler.StartScheduler();

            scheduler.ScheduleJob<BillingJob>("billing", "*/3 * * * * ?");
            scheduler.ScheduleJob<MonitoringJob>("monitoring", "*/5 * * * * ?");

        }

         
    }
}