using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Service.IService;

namespace Crytex.Background.Statistic
{
    public class StatisticJob
    {
        private IStatisticService _statisticService { get; set; }
    
        public StatisticJob(TypeStatistic typeStatistic, IStatisticService statisticService)
        {
            this._statisticService = statisticService;
            this._statisticService.CalculateStatistic(typeStatistic);
        }
    }
}
