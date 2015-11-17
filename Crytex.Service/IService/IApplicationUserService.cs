﻿using System.Collections.Generic;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IApplicationUserService
    {
        List<ApplicationUser> GetAll();

        IPagedList<ApplicationUser> GetPage(int pageSize, int pageIndex, string userName, string email);

        ApplicationUser GetUserById(string id);

        void DeleteUser(ApplicationUser user);
    }
}