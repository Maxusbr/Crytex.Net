using Crytex.Background.Statistic;
using Crytex.Model.Models;
using Crytex.Service.IService;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class NumberRunningMachineJob : IJob
    {
        private IStatisticJobFactory _statisticJobFactory { get; set; }
        public NumberRunningMachineJob(IStatisticJobFactory statisticJobFactory)
        {
            this._statisticJobFactory = statisticJobFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            _statisticJobFactory.CreateStatisticJob(TypeStatistic.NumberRunningMachine);
            Console.WriteLine("It's billing NumberRunningMachineJob!");
        }
    }
}