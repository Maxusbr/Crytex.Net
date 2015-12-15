using System.Collections.Generic;
using Crytex.Model.Models;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IApplicationUserService
    {
        List<ApplicationUser> GetAll();

        IPagedList<ApplicationUser> GetPage(int pageNumber, int pageSize, ApplicationUserSearchParams searchParams = null);

        ApplicationUser GetUserById(string id);

        List<ApplicationUser> Search(string searchParam);

        void DeleteUser(string id);

        void UpdateStateUser(UpdateUserState data);
    }
}