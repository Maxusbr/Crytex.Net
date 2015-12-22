using System;
using System.Linq;
using Crytex.Background.Statistic;
using Crytex.Model.Models;
using System.Threading;
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
    using Tasks.SubscriptionVm;

    public static class Program
    {
        public static void Main(string[] args)
        {
            LoggerCrytex.SetSource(SourceLog.Background);
            UnityConfig.Configure();
            var scheduler = UnityConfig.Resolve<ISchedulerJobs>();

            var Thread = new Thread((() =>
            {
                var taskManager = UnityConfig.Resolve<ITaskManager>();
                taskManager.RunTasks();
            }));
            Thread.Start();

            scheduler.StartScheduler();

            //var statisticData = new List<KeyValuePair<string, Object>>
            //{
            //    new KeyValuePair<string, Object>("typeStatistic", TypeStatistic.NumberRunningMachine)
            //};
            //scheduler.ScheduleJob<StatisticJob>("NumberRunningMachine", "0 */1 * 1/1 * ? *", statisticData);

            //statisticData[0] = new KeyValuePair<string, Object>("typeStatistic", TypeStatistic.NumberStoppedMachine);
            //scheduler.ScheduleJob<StatisticJob>("NumberStoppedMachine", "0 */1 * 1/1 * ? *", statisticData);

            //statisticData[0] = new KeyValuePair<string, Object>("typeStatistic", TypeStatistic.NumberTasksCompletedDuringPeriod);
            //scheduler.ScheduleJob<StatisticJob>("NumberTasksCompletedDuringPeriod", "0 0 */1 1/1 * ? *", statisticData);

            //statisticData[0] = new KeyValuePair<string, Object>("typeStatistic", TypeStatistic.NumberTasks);
            //scheduler.ScheduleJob<StatisticJob>("NumberTasksJob", "0 0/5 * 1/1 * ? *", statisticData);

            //statisticData[0] = new KeyValuePair<string, Object>("typeStatistic", TypeStatistic.NumberUsers);
            //scheduler.ScheduleJob<StatisticJob>("NumberUsersJob", "0 0 0 1/1 * ? *", statisticData);

            //statisticData[0] = new KeyValuePair<string, Object>("typeStatistic", TypeStatistic.AverageDelayStartEndTasksInPeriod);
            //scheduler.ScheduleJob<StatisticJob>("AverageDelayStartEndTasksInPeriod", "0 */10 * 1/1 * ? *", statisticData);

            //statisticData[0] = new KeyValuePair<string, Object>("typeStatistic", TypeStatistic.UsersWithLeastOneRunningMachine);
            //scheduler.ScheduleJob<StatisticJob>("UsersWithLeastOneRunningMachine", "0 0 0 1/1 * ? *", statisticData);
            //scheduler.ScheduleJob<MonitoringJob>("monitoring", "1/10 * * * * ?");

            //scheduler.ScheduleJob<BillingJob>("billing", "*/3 * * * * ?");

            //var emai = scheduler.ScheduleJob<EmailSendJob>("emailSending", "0 */5 * * * ?");
            //scheduler.TriggerJob(emai);

            //scheduler.ScheduleJob<TaskExecutorUpdateJob>("task executor update", "1/10 * * * * ?");
            scheduler.ScheduleJob<UsageSubscriptionVmJob>("task wait for payment subs", "1/30 * * * * ?");
            //scheduler.ScheduleJob<StatisticJob>("UsersWithLeastOneRunningMachine", "0 0 0 1/1 * ? *", statisticData);
            //scheduler.ScheduleJob<NetTrafficCounterUpdateJob>("net traffic", "0 */15 * * * ?");

            LoggerCrytex.Logger.Info("Hello from Background");
        }


    }
}