using Crytex.Model.Models.Biling;
using Crytex.Model.Models.WebHostingModels;
using Crytex.Service.Model;
using PagedList;
using System;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IWebHostingService
    {
        WebHosting GetById(Guid webHostingId);
        WebHosting BuyNewHosting(BuyWebHostingParams buyParams);
        IPagedList<WebHostingPayment> GetWebHostingPaymentsPaged(int pageNumber, int pageSize, string userId = null, Guid? webHostingId = null);
        void ProlongateWebHosting(Guid webHostingId, int monthCount);
        void AutoProlongateWebHosting(Guid webHostingId, int monthCount);
        void UpdateWebHosting(Guid webHostingId, string name = null, bool? autoProlongation = null);
        IEnumerable<WebHosting> GetAllByStatusAndExpireDate(WebHostingStatus status, DateTime? dateFrom = null, DateTime? dateTo = null);
        void UpdateWebHostingStatus(Guid id, WebHostingStatus newStatus);
        void PrepareHostingForDeletion(Guid id);
    }
}
