using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Model
{
    public class StatisticSummary
    {
        public int NumberUsers { get; set; }
        public int NumberActiveUsers { get; set; }
        public int SumBalanceUsers { get; set; }
        public int EarnedToday { get; set; }
        public int EarnedMonth { get; set; }
        public int SpentMoneyUsers { get; set; }
        public int NumberMachine { get; set; }
        public int NumberRunningMachine { get; set; }
        public int CreatedMachinesToday { get; set; }
        public int CreatedMachinesTodayInAmount { get; set; }
    }
}
