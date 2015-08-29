using PagedList;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.IService
{
    public interface IUserVmService
    {
        UserVm GetVmById(Guid id);

        IPagedList<UserVm> GetPage(int pageNumber, int pageSize, string userId);
    }
}
