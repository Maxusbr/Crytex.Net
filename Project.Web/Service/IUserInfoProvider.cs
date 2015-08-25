using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Web.Service
{
    public interface IUserInfoProvider
    {
        string GetUserId();
        ApplicationUser GetCurrentUser();
        IEnumerable<string> GetRolesForCurrentUser();
        bool IsCurrentUserInRole(string roleName);
    }
}
