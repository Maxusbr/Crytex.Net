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
            taskManager.RunTasks();
            
            scheduler.StartScheduler();
            scheduler.ScheduleJob<NumberRunningMachineJob>("NumberRunningMachineJob", "0 */1 * 1/1 * ? *");
            scheduler.ScheduleJob<NumberStoppedMachineJob>("NumberStoppedMachineJob", "0 */1 * 1/1 * ? *");
            scheduler.ScheduleJob<NumberTasksCompletedDuringPeriodJob>("NumberTasksCompletedDuringPeriodJob", "0 0 */1 1/1 * ? *");
            scheduler.ScheduleJob<NumberTasksJob>("NumberTasksJob", "0 0/5 * 1/1 * ? *");
            scheduler.ScheduleJob<NumberUsersJob>("NumberUsersJob", "0 0 0 1/1 * ? *");
            scheduler.ScheduleJob<AverageDelayStartEndTasksInPeriodJob>("AverageDelayStartEndTasksInPeriodJob", "0 */10 * 1/1 * ? *");
            scheduler.ScheduleJob<UsersWithLeastOneRunningMachineJob>("UsersWithLeastOneRunningMachineJob", "0 0 0 1/1 * ? *");
            scheduler.ScheduleJob<MonitoringJob>("monitoring", "*/30 * * * * ?");
            //scheduler.ScheduleJob<BillingJob>("billing", "*/3 * * * * ?");

            //var emai = scheduler.ScheduleJob<EmailSendJob>("emailSending", "0 */5 * * * ?");
            //scheduler.TriggerJob(emai);
            // scheduler.ScheduleJob<TaskExecutorUpdateJob>("task executor update", "0 * * * * ?");

            LoggerCrytex.Logger.Info("Hello from Background");
        }

         
    }
}