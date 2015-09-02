using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface IApplicationUserService
    {
        List<ApplicationUser> GetAll();


        List<ApplicationUser> GetPage(int pageSize, int pageIndex, string userName, string email);

        ApplicationUser GetUserById(string id);
    }
}