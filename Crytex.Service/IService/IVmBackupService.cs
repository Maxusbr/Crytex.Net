using Crytex.Model.Models;
using PagedList;
using System;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IVmBackupService
    {
        VmBackup GetById(Guid guid);
        IPagedList<VmBackup> GetPage(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null, Guid? vmId = null);
        VmBackup Create(Guid subscriptionVmId, string backupName);
        IEnumerable<VmBackup> GetByVmId(Guid id);
        void Delete(Guid vmBackupId);
        void UpdateBackupStatus(Guid backupGuid, VmBackupStatus status);
        void DeleteBackupDbEntity(Guid backupGuid);
    }
}
