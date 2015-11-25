using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IPhoneCallRequestService
    {
        PhoneCallRequest Create(PhoneCallRequest request);
        PhoneCallRequest GetById(int id);
        IPagedList<PhoneCallRequest> GetPage(int pageNumber, int pageSize);
        void Update(int id, PhoneCallRequest request);
    }
}
