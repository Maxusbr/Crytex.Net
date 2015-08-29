using Crytex.Model.Models;
using System.Collections.Generic;

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
