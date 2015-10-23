using Crytex.Background.Statistic;
using Crytex.Model.Models;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class NumberUsersJob : IJob
    {
        private IStatisticJobFactory _statisticJobFactory { get; set; }
        public NumberUsersJob(IStatisticJobFactory statisticJobFactory)
        {
            this._statisticJobFactory = statisticJobFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            _statisticJobFactory.CreateStatisticJob(TypeStatistic.NumberUsers);
            Console.WriteLine("It's billing NumberUsersJob!");
        }
    }
}