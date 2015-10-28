namespace Crytex.Background
{
    using Crytex.Core;
    using Crytex.Data.Infrastructure;
    using Crytex.Data.Repository;
    using Crytex.ExecutorTask;
    using Crytex.ExecutorTask.TaskHandler;
    using Crytex.Service.Service;
    using Scheduler;
    using System.Collections.Generic;
    using Tasks;

    public static class Program
    {
        public static void Main(string[] args)
        {
            LoggerCrytex.SetSource(SourceLog.Background);
            UnityConfig.Configure();
            var scheduler = UnityConfig.Resolve<ISchedulerJobs>();
            var taskManager = UnityConfig.Resolve<ITaskManager>();
            //taskManager.RunTasks();

            scheduler.StartScheduler();

            //scheduler.ScheduleJob<BillingJob>("billing", "*/3 * * * * ?");
            scheduler.ScheduleJob<MonitoringJob>("monitoring", "*/1 * * * * ?");
            //var emai = scheduler.ScheduleJob<EmailSendJob>("emailSending", "0 */5 * * * ?");
            //scheduler.TriggerJob(emai);
            scheduler.ScheduleJob<TaskExecutorUpdateJob>("task executor update", "*/1 * * * * ?");

            LoggerCrytex.Logger.Info("Hello from Background");
        }

         
    }
}