using PagedList;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.IService
{
    public interface IUserVmService
    {
        UserVm GetVmById(int id);

        IPagedList<UserVm> GetPage(int pageNumber, int pageSize);
    }
}
