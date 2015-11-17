using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class StatisticViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Value { get; set; }
        public TypeStatistic Type { get; set; }
    }
}
