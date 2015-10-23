using Crytex.Background.Statistic;
using Crytex.Model.Models;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class UsersWithLeastOneRunningMachineJob : IJob
    {
        private IStatisticJobFactory _statisticJobFactory { get; set; }
        public UsersWithLeastOneRunningMachineJob(IStatisticJobFactory statisticJobFactory)
        {
            this._statisticJobFactory = statisticJobFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            _statisticJobFactory.CreateStatisticJob(TypeStatistic.UsersWithLeastOneRunningMachine);
            Console.WriteLine("It's billing UsersWithLeastOneRunningMachineJob!");
        }
    }
}