using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Background.Statistic;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Quartz;

namespace Crytex.Background.Tasks
{
    public class StatisticJobFactory: IStatisticJobFactory
    {
        private IStatisticService _statisticService { get; set; }
        public StatisticJobFactory(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        public void CreateStatisticJob(TypeStatistic typeStatistic)
        {
            new StatisticJob(typeStatistic, _statisticService);
        }
    }
}
