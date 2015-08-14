namespace Crytex.Background.Scheduler
{
    using System.Collections.Generic;
    using Quartz;

    public interface ISchedulerJobs {
        void StartScheduler();

        void StopScheduler();

        JobKey ScheduleJob<T>(string name, string cronExpression, List<KeyValuePair<string, object>> data = null) where T : IJob;

        List<JobKey> GetJobKeys();

        bool CheckExistJob(JobKey jobKey);

        void PauseJob(JobKey jobKey);

        void PauseAllJob();

        void ResumeJob(JobKey jobKey);

        void ResumeAllJob();

        bool RemoveJob(JobKey jobKey);

        void TriggerJob(JobKey jobKey);

        void Clear();

        void ResumeAllTriggers();

        void PauseAllTriggers();
    }
}