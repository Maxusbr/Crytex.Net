using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Quartz;

namespace Crytex.Background.Statistic
{
    public class StatisticJob: IJob
    {
        private IStatisticService _statisticService { get; set; }
        private TypeStatistic typeStatistic { get; set; }

        public StatisticJob(IStatisticService statisticService)
        {
            this._statisticService = statisticService;
        }

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;
            typeStatistic = (TypeStatistic)dataMap["typeStatistic"];

            this._statisticService.CalculateStatistic(typeStatistic);
            Console.WriteLine("It's billing " + context.JobDetail.Key.Name);
        }
    }
}
