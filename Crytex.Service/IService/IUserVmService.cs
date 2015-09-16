using PagedList;
using Crytex.Model.Models;
using System;

namespace Crytex.Service.IService
{
    public interface IUserVmService
    {
        UserVm GetVmById(Guid id);

        IPagedList<UserVm> GetPage(int pageNumber, int pageSize, string userId);

        void CreateVm(UserVm userVm);
    }
}
