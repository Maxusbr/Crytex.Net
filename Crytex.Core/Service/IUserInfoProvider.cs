using Crytex.Model.Models;
using System.Collections.Generic;

namespace Crytex.Core.Service
{
    public interface IUserInfoProvider
    {
        string GetUserId();
        ApplicationUser GetCurrentUser();
        IEnumerable<string> GetRolesForCurrentUser();
        bool IsCurrentUserInRole(string roleName);
        bool IsCurrentUserInAnyRole(List<string> roleName);

        bool IsCurrentUserAdmin();

        bool IsCurrentUserSupport();
    }
}
