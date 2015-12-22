using Crytex.Service.IService;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;
using System;
using Crytex.Data.Infrastructure;
using System.Linq.Expressions;
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

        public virtual IPagedList<FixedSubscriptionPayment> GetPage(int pageNumber, int pageSize, FixedSubscriptionPaymentSearchParams searchParams)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<FixedSubscriptionPayment, bool>> where = x => true;

            if(searchParams.From != null)
            {
                where = where.And(x => x.Date >= searchParams.From);
            }
            if(searchParams.To != null)
            {
                where = where.And(x => x.Date <= searchParams.To);
            }
            if(searchParams.SubscriptionVmId != null)
            {
                where = where.And(x => x.SubscriptionVmId == searchParams.SubscriptionVmId);
            }
            if(searchParams.UserId != null)
            {
                where = where.And(x => x.SubscriptionVm.UserId == searchParams.UserId);
            }

            var pagedList = this._paymentRepo.GetPage(pageInfo, where, x => x.Date, false, s => s.SubscriptionVm, s => s.SubscriptionVm.UserVm);

            return pagedList;
        }
    }
}
