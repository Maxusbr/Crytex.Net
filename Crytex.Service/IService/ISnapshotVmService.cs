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
        /// <summary>
        /// Обвновляет текущий снепшот машины и родительский снепшот для снепшота с указанным Id
        /// </summary>
        void ActivateNewlyCreatedSnapshot(Guid snapshotId);
        IEnumerable<SnapshotVm> GetAllActive();
        void PrepareSnapshotForDeletion(Guid snapshotId, bool deleteWithChildrens);
        SnapshotVm GetById(Guid snapshotId);
        void DeleteSnapshot(Guid snapshotId, bool deleteWithChildrens);
        void SetLoadedSnapshotActive(Guid snapshotId);
    }
}
