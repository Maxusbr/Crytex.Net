using Crytex.Model.Models.WebHosting;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IWebHostingTariffService
    {
        IPagedList<WebHostingTariff> GetPage(int pageNumber, int pageSize);
        WebHostingTariff Create(WebHostingTariff tariff);
    }
}