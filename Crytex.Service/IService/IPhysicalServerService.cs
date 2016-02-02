using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IPhysicalServerService
    {
        PhysicalServer CreatePhysicalServer(CreatePhysicalServerParam serverParam);
        void UpdateOptionsAviable(PhysicalServerOptionsAviableParams optionsParams);
        PhysicalServerOption CreateOrUpdateOption(PhysicalServerOptionsParams optionsParams);
        void CreateOrUpdateOptions(IEnumerable<PhysicalServerOptionsParams> optionsParams);
        void DeletePhysicalServer(Guid serverId);
        void DeletePhysicalServerOption(Guid optionId);
        void DeleteBoughtPhysicalServer(Guid serverId);

        BoughtPhysicalServer BuyPhysicalServer(BuyPhysicalServerParam serverParam);
        void UpdateBoughtPhysicalServerState(PhysicalServerStateParams serverParam);
        BoughtPhysicalServer UpdateBoughtPhysicalServer(UpdatePhysicalServerParam serverParam);

        IPagedList<PhysicalServer> GetPagePhysicalServer(int pageNumber, int pageSize, PhysicalServerSearchParams searchParams = null);
        IPagedList<PhysicalServerOption> GetPagePhysicalServerOption(int pageNumber, int pageSize, PhysicalServerOptionSearchParams searchParams = null);
        IPagedList<BoughtPhysicalServer> GetPageBoughtPhysicalServer(int pageNumber, int pageSize, BoughtPhysicalServerSearchParams searchParams = null);

        PhysicalServer GetReadyPhysicalServer(Guid serverId);
        PhysicalServer GetAviablePhysicalServer(Guid serverId);

        BoughtPhysicalServer GetBoughtPhysicalServer(Guid serverId);
        List<BoughtPhysicalServer> GetPhysicalServerByStatus(BoughtPhysicalServerStatus status);
        List<BoughtPhysicalServer> GetAllUsagePhysicalServer();
        void AutoProlongatePhysicalServer(Guid serverId);
        List<BoughtPhysicalServer> GetPhysicalServerMessageSend();
        void CompleteSendMessage(Guid serverId);
    }
}
