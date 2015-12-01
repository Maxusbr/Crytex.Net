using Crytex.Model.Models;
using System;

namespace Crytex.Service.IService
{
    public interface INetTrafficCounterService
    {
        NetTrafficCounter GetCurrentDayCounterForVm(Guid machineId);
        NetTrafficCounter CreateCounterForToday(Guid machineId);
        void UpdateCounter(NetTrafficCounter counter);
    }
}
