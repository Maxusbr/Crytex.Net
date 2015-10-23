using Crytex.Background.Statistic;
using Crytex.Model.Models;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class NumberTasksJob : IJob
    {
        private IStatisticJobFactory _statisticJobFactory { get; set; }
        public NumberTasksJob(IStatisticJobFactory statisticJobFactory)
        {
            this._statisticJobFactory = statisticJobFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            _statisticJobFactory.CreateStatisticJob(TypeStatistic.NumberTasks);
            Console.WriteLine("It's billing NumberTasksJob!");
        }
    }
}