using Crytex.Model.Models;
using PagedList;
using System;

namespace Crytex.Service.IService
{
    public interface IVmBackupService
    {
        VmBackup GetById(Guid guid);
        IPagedList<VmBackup> GetPage(int pageNumber, int pageSize, DateTime? from, DateTime? to);
        void Create(VmBackup newBackupDbEntity);
    }
}
