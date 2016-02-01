using Crytex.Model.Models.WebHostingModels;
using PagedList;
using System;

namespace Crytex.Service.IService
{
    public interface IWebHostingTariffService
    {
        WebHostingTariff GetById(Guid id);
        IPagedList<WebHostingTariff> GetPage(int pageNumber, int pageSize);
        WebHostingTariff Create(WebHostingTariff tariff);
    }
}