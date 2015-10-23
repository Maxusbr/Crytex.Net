using Crytex.Background.Statistic;
using Crytex.Model.Models;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class NumberStoppedMachineJob : IJob
    {
        private IStatisticJobFactory _statisticJobFactory { get; set; }
        public NumberStoppedMachineJob(IStatisticJobFactory statisticJobFactory)
        {
            this._statisticJobFactory = statisticJobFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            _statisticJobFactory.CreateStatisticJob(TypeStatistic.NumberStoppedMachine);
            Console.WriteLine("It's billing NumberStoppedMachineJob!");
        }
    }
}