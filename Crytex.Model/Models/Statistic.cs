using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class Statistic : BaseEntity
    {
        public DateTime Date { get; set; }
        public float Value { get; set; }
        public TypeStatistic Type { get; set; }
    }

    public enum TypeStatistic
    {
        NumberRunningMachine = 0,
        NumberStoppedMachine = 1,
        NumberUsers = 2,
        UsersWithLeastOneRunningMachine = 3,
        NumberTasks = 4,
        NumberTasksCompletedDuringPeriod,
        AverageDelayStartEndTasksInPeriod
    }
}
