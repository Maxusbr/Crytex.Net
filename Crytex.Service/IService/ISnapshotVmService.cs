using System;
using System.Collections.Generic;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface ISnapshotVmService
    {
        SnapshotVm GetById(Guid snapshotId);
        IPagedList<SnapshotVm> GetAllByVmId(Guid VmId, int pageNumber, int pageSize);
        IEnumerable<SnapshotVm> GetAllActive();
        SnapshotVm Create(SnapshotVm newSnapShot);
        /// <summary>
        /// Обвновляет текущий снепшот машины и родительский снепшот для снепшота с указанным Id
        /// </summary>
        void ActivateNewlyCreatedSnapshot(Guid snapshotId);
        void PrepareSnapshotForDeletion(Guid snapshotId, bool deleteWithChildrens);
        void DeleteSnapshot(Guid snapshotId, bool deleteWithChildrens);
        void LoadSnapshot(Guid snapshotId);
        void SetLoadedSnapshotActive(Guid snapshotId);
        void RenameSnapshot(Guid id, string newName);
    }
}
