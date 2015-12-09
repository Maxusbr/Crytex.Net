using Crytex.Model.Models;
using PagedList;
using System;

namespace Crytex.Service.IService
{
    public interface IVmBackupService
    {
        VmBackup GetById(Guid guid);
        IPagedList<VmBackup> GetPage(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null, Guid? vmId = null);
        void Create(VmBackup newBackupDbEntity);
    }
}
