using System;
using System.Collections.Generic;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface ISnapshotVmService
    {
        IPagedList<SnapshotVm> GetAllByVmId(Guid VmId, int pageNumber, int pageSize);
        SnapshotVm Create(SnapshotVm newSnapShot);
        void ChangeSnapshotStatus(Guid snapshotGuid, SnapshotStatus newStatus);
    }
}
