using Crytex.Model.Models.Biling;
using Crytex.Model.Models.WebHostingModels;
using Crytex.Service.Model;
using PagedList;
using System;

namespace Crytex.Service.IService
{
    public interface IWebHostingService
    {
        WebHosting GetById(Guid webHostingId);
        WebHosting BuyNewHosting(BuyWebHostingParams buyParams);
        IPagedList<WebHostingPayment> GetWebHostingPaymentsPaged(int pageNumber, int pageSize, string userId = null, Guid? webHostingId = null);
        void ProlongateWebHosting(Guid webHostingId, int monthCount);
        void UpdateWebHosting(Guid webHostingId, string name = null, bool? autoProlongation = null);
    }
}
