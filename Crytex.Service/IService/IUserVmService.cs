using PagedList;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using Crytex.Service.Model;

namespace Crytex.Service.IService
{
    public interface IUserVmService
    {
        UserVm GetVmById(Guid id);

        IPagedList<UserVm> GetPage(int pageNumber, int pageSize, UserVmSearchParams searchParams = null);

        IPagedList<UserVm> GetPage(int pageNumber, int pageSize, string userId = null);

        Guid CreateVm(UserVm userVm);

        void UpdateVm(Guid vmId, int? cpu = null, int? hdd = null, int? ram = null);
        void UpdateVm(UserVm vm);

        void UpdateVmStatus(Guid vmId, TypeChangeStatus status);

        IEnumerable<UserVm> GetVmByListId(List<Guid> listId);

        IEnumerable<UserVm> GetAllVmsHyperV();

        IEnumerable<UserVm> GetAllVmsVmWare();

        void AddIpAddressesToVm(Guid vmId, IEnumerable<VmIpAddress> addresses);
    }
}
