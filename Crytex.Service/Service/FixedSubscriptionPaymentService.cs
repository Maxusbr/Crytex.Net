using Crytex.Service.IService;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;
using System;
using Crytex.Data.Infrastructure;
using System.Linq.Expressions;
using Crytex.Model.Exceptions;
using Crytex.Service.Extension;

namespace Crytex.Service.Service
{
    class FixedSubscriptionPaymentService : IFixedSubscriptionPaymentService
    {
        private readonly IFixedSubscriptionPaymentRepository _paymentRepo;

        public FixedSubscriptionPaymentService(IFixedSubscriptionPaymentRepository paymentRepo)
        {
            this._paymentRepo = paymentRepo;
        }

        public virtual IPagedList<FixedSubscriptionPayment> GetPage(int pageNumber, int pageSize, FixedSubscriptionPaymentSearchParams searchParams = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<FixedSubscriptionPayment, bool>> where = x => true;

            if (searchParams != null)
            {
                Guid SubscriptionVmId = Guid.Empty;
                if (searchParams.SubscriptionVmId != null)
                {
                    if (!Guid.TryParse(searchParams.SubscriptionVmId, out SubscriptionVmId))
                    {
                        throw new InvalidIdentifierException(string.Format("Invalid Guid format for {0}", searchParams.SubscriptionVmId));
                    }
                }

                if (searchParams.DateFrom != null)
                {
                    where = where.And(x => x.Date >= searchParams.DateFrom);
                }
                if (searchParams.DateTo != null)
                {
                    where = where.And(x => x.Date <= searchParams.DateTo);
                }
                if (searchParams.OperatingSystem != null)
                {
                    where = where.And(x => x.Tariff.OperatingSystem == searchParams.OperatingSystem);
                }
                if (searchParams.Virtualization != null)
                {
                    where = where.And(x => x.Tariff.Virtualization == searchParams.Virtualization);
                }
                if (SubscriptionVmId != Guid.Empty)
                {
                    where = where.And(x => x.SubscriptionVmId == SubscriptionVmId);
                }
                if (searchParams.UserId != null)
                {
                    where = where.And(x => x.SubscriptionVm.UserId == searchParams.UserId);
                }
            }

            var pagedList = this._paymentRepo.GetPage(pageInfo, where, x => x.Date, false, s => s.SubscriptionVm,
                s => s.SubscriptionVm.UserVm, s => s.SubscriptionVm.User, s => s.Tariff);

            return pagedList;
        }
    }
}
