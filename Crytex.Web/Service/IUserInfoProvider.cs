using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Web.Service
{
    public interface IUserInfoProvider
    {
        string GetUserId();
        ApplicationUser GetCurrentUser();
        IEnumerable<string> GetRolesForCurrentUser();
        bool IsCurrentUserInRole(string roleName);
    }
}
