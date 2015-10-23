using Crytex.Background.Statistic;
using Crytex.Model.Models;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class NumberTasksCompletedDuringPeriodJob : IJob
    {
        private IStatisticJobFactory _statisticJobFactory { get; set; }
        public NumberTasksCompletedDuringPeriodJob(IStatisticJobFactory statisticJobFactory)
        {
            this._statisticJobFactory = statisticJobFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            _statisticJobFactory.CreateStatisticJob(TypeStatistic.NumberTasksCompletedDuringPeriod);
            Console.WriteLine("It's billing NumberTasksCompletedDuringPeriodJob!");
        }
    }
}