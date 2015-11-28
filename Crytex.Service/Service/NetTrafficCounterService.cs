using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;

namespace Crytex.Service.Service
{
    public class NetTrafficCounterService
    {
        private readonly INetTrafficCounterRepository _netTrafficCounterRepo;
        private readonly IUnitOfWork _unitOfWork;

        public NetTrafficCounterService(INetTrafficCounterRepository counterRepo, IUnitOfWork unitOfWork)
        {
            this._netTrafficCounterRepo = counterRepo;
            this._unitOfWork = unitOfWork;
        }
    }
}
