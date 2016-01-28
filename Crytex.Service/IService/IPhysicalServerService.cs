using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;
using Crytex.Model.Models;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IPhysicalServerService
    {
        PhysicalServer CreatePhysicalServer(CreatePhysicalServerParam serverParam);
        void UpdateOptionsAviable(Guid serverId, IEnumerable<PhysicalServerOptionsParams> optionsParams);
        void AddOptionsAviable(Guid serverId, IEnumerable<PhysicalServerOptionsParams> optionsParams);
        PhysicalServerOption CreateOrUpdateOptions(PhysicalServerOptionsParams optionsParams);
        void CreateOrUpdateOptions(IEnumerable<PhysicalServerOptionsParams> optionsParams);
        void DeletePhysicalServer(Guid serverId);
        void DeletePhysicalServerOption(Guid optionId);

        void BuyPhysicalServer(BuyPhysicalServerParam serverParam);
        void UpdateBoughtPhysicalServerState(Guid serverId, BoughtPhysicalServerStatus state);


        IPagedList<PhysicalServer> GetPagePhysicalServer(int pageNumber, int pageSize, PhysicalServerSearchParams searchParams);
        IPagedList<PhysicalServerOption> GetPagePhysicalServerOption(int pageNumber, int pageSize, PhysicalServerOptionSearchParams searchParams);
        IPagedList<BoughtPhysicalServer> GetPageBoughtPhysicalServer(int pageNumber, int pageSize, BoughtPhysicalServerSearchParams searchParams);

        PhysicalServer GetReadyPhysicalServer(Guid serverId);
        PhysicalServer GetAviablePhysicalServer(Guid serverId);

        BoughtPhysicalServer GetBoughtPhysicalServer(Guid serverId);

    }
}
