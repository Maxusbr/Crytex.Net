using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IFixedSubscriptionPaymentService
    {
        IPagedList<FixedSubscriptionPayment> GetPage(int pageNumber, int pageSize, FixedSubscriptionPaymentSearchParams searchParams = null);
    }
}
