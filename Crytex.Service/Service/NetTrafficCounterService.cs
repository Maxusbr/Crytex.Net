using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Service.IService;
using Crytex.Model.Models;
using System;

namespace Crytex.Service.Service
{
    public class NetTrafficCounterService : INetTrafficCounterService
    {
        private readonly INetTrafficCounterRepository _netTrafficCounterRepo;
        private readonly IUnitOfWork _unitOfWork;

        public NetTrafficCounterService(INetTrafficCounterRepository counterRepo, IUnitOfWork unitOfWork)
        {
            this._netTrafficCounterRepo = counterRepo;
            this._unitOfWork = unitOfWork;
        }

        public NetTrafficCounter CreateCounterForToday(Guid machineId)
        {
            var today = DateTime.Today;
            var counter = new NetTrafficCounter
            {
                CountingPeriodStartDate = today,
                MachineId = machineId,
                PeriodType = Crytex.Model.Enums.CountingPeriodType.Day,
                ReceiveKiloBytes = 0,
                TransmittedKiloBytes = 0,
                LastUpdateDate = null
            };

            this._netTrafficCounterRepo.Add(counter);
            this._unitOfWork.Commit();

            return counter;
        }

        public NetTrafficCounter GetCurrentDayCounterForVm(Guid machineId)
        {
            var today = DateTime.Today;
            var counter = this._netTrafficCounterRepo.Get(c => c.CountingPeriodStartDate == today);

            return counter;
        }

        public void UpdateCounter(NetTrafficCounter counter)
        {
            var counterToUpdate = this._netTrafficCounterRepo.GetById(counter.Id);
            counterToUpdate.ReceiveKiloBytes = counter.ReceiveKiloBytes;
            counterToUpdate.TransmittedKiloBytes = counter.TransmittedKiloBytes;
            counterToUpdate.LastUpdateDate = counter.LastUpdateDate;

            this._netTrafficCounterRepo.Update(counterToUpdate);
            this._unitOfWork.Commit();
        }
    }
}
