namespace Crytex.Background.Scheduler
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Logging;
    using Common.Logging.Simple;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.Matchers;
    using Quartz.Impl.Triggers;

    public class SchedulerJobs : ISchedulerJobs
    {
        List<JobKey> _jobKeys { get; set; }

        IScheduler _scheduler { get; set; }

        public SchedulerJobs()
        {
            _jobKeys = new List<JobKey>();
            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter { Level = LogLevel.Info };
        }

        #region Scheduler 

        public void StartScheduler() => _scheduler.Start();

        public void StopScheduler() => _scheduler.Shutdown();

        #endregion

        #region Jobs

        public JobKey ScheduleJob<T>(string name, string cronExpression, List<KeyValuePair<string, object>> data = null) where T : IJob
        {
            var jobDetails = JobBuilder.Create<T>().WithIdentity(name + "Job", name + "Group").Build();

            if (data != null)
                foreach (var keyValuePair in data)
                    jobDetails.JobDataMap[keyValuePair.Key] = keyValuePair.Value;

            //TODO: WithMisfireHandlingInstructionDoNothing method does'n work, when you do pause and after resume fires which were in this timespan will be fired - it's wrong
            var trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity(name + "Trigger", name + "Group")
                .WithCronSchedule(cronExpression, x=>x.WithMisfireHandlingInstructionDoNothing().Build())
                .Build();
            _scheduler.ScheduleJob(jobDetails, trigger);
            _jobKeys.Add(jobDetails.Key);

            return jobDetails.Key;
        }

        public List<JobKey> GetJobKeys() => _jobKeys;

        public bool CheckExistJob(JobKey jobKey) => _scheduler.CheckExists(jobKey);

        public void PauseJob(JobKey jobKey)
        {
            var triggerKey = GetTriggerKey(jobKey);

            _scheduler.PauseTrigger(triggerKey);
            _scheduler.PauseJob(jobKey);
        }

        public void PauseAllJob() => _jobKeys.ForEach(PauseJob);

        public void ResumeJob(JobKey jobKey)
        {
            var triggerKey = GetTriggerKey(jobKey);

            _scheduler.ResumeTrigger(triggerKey);
            _scheduler.ResumeJob(jobKey);
        }

        public void ResumeAllJob() => _jobKeys.ForEach(ResumeJob);

        public bool RemoveJob(JobKey jobKey)
        {
            if (_jobKeys.RemoveAll(x => x.Name == jobKey.Name && x.Group == jobKey.Group) > 0)
                return _scheduler.DeleteJob(jobKey);
            return false;
        }

        public void TriggerJob(JobKey jobKey) => _scheduler.TriggerJob(jobKey);

        public void Clear()
        {
            _jobKeys.Clear();
            _scheduler.Clear();
        }

        #endregion

        #region Triggers

        public void RescheduleJob(JobKey jobKey, string cronExpression)
        {
            var triggerKey = GetTriggerKey(jobKey);
            _scheduler.RescheduleJob(triggerKey, new CronTriggerImpl(triggerKey.Name, triggerKey.Group, cronExpression));
        }

        public void ResumeAllTriggers() => _scheduler.ResumeAll();

        public void PauseAllTriggers() => _scheduler.PauseAll();

        #endregion


        #region Helpers

        TriggerKey GetTriggerKey(JobKey jobKey)
        {
            return _scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup()).FirstOrDefault(x => x.Group == jobKey.Group);
        }

        #endregion

    }
}