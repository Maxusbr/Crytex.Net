using PagedList;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IUserVmService
    {
        UserVm GetVmById(Guid id);

        IPagedList<UserVm> GetPage(int pageNumber, int pageSize, string userId);

        Guid CreateVm(UserVm userVm);

        void UpdateVm(Guid vmId, int? cpu = null, int? hdd = null, int? ram = null);

        void UpdateVmStatus(Guid vmId, TypeChangeStatus status);

        IEnumerable<UserVm> GetVmByListId(List<Guid> listId);

        IEnumerable<UserVm> GetAllVmsHyperV();
    }
}
