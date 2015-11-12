using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Model
{
    public enum StatisticType
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

