using Crytex.Model.Models.WebHosting;
using System;

namespace Crytex.Service.IService
{
    public interface IWebHostingService
    {
        WebHosting BuyNewHosting(Guid hostingTariffId, string userId);
    }
}
