using Crytex.Background.Statistic;
using Crytex.Model.Models;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class AverageDelayStartEndTasksInPeriodJob : IJob
    {
        private IStatisticJobFactory _statisticJobFactory { get; set; }
        public AverageDelayStartEndTasksInPeriodJob(IStatisticJobFactory statisticJobFactory)
        {
            this._statisticJobFactory = statisticJobFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            _statisticJobFactory.CreateStatisticJob(TypeStatistic.AverageDelayStartEndTasksInPeriod);
            Console.WriteLine("It's billing AverageDelayStartEndTasksInPeriodJob!");
        }
    }
}