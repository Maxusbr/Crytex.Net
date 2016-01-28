using Crytex.Model.Models.WebHostingModels;
using Crytex.Service.Model;

namespace Crytex.Service.IService
{
    public interface IWebHostingService
    {
        WebHosting BuyNewHosting(BuyWebHostingParams buyParams);
    }
}
