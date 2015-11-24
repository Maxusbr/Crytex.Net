using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;

namespace Crytex.Service.Service
{
    public class SubscriptionVmService : ISubscriptionVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionVmRepository _subscriptionVmRepository;
        public SubscriptionVmService(IUnitOfWork unitOfWork, ISubscriptionVmRepository subscriptionVmRepository)
        {
            this._unitOfWork = unitOfWork;
            this._subscriptionVmRepository = subscriptionVmRepository;
        }

    }
}
