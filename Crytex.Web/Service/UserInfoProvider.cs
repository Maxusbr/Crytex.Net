using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Crytex.Model.Models;

namespace Crytex.Web.Service
{
    public class UserInfoProvider : IUserInfoProvider
    {
        private IIdentity _identity;
        private ApplicationUserManager _userManager;
        public UserInfoProvider(IIdentity identity, ApplicationUserManager userManager)
        {
            this._identity = identity;
            this._userManager = userManager;
        }

        public string GetUserId()
        {
            var userId = this._identity.GetUserId();
            return userId ;
        }

        public bool IsAuth()
        {
            return this._identity.IsAuthenticated;
        }


        public ApplicationUser GetCurrentUser()
        {
            var user = this._userManager.FindById(GetUserId());
            return user;
        }


        public IEnumerable<string> GetRolesForCurrentUser()
        {
            var roles = this._userManager.GetRoles(this.GetUserId());
            return roles;
        }


        public bool IsCurrentUserInRole(string roleName)
        {
            bool isIn = this._userManager.IsInRole(this.GetUserId(), roleName);
            return isIn;
        }

        public bool IsCurrentUserInAnyRole(List<string> roleName)
        {
            return roleName.Any(IsCurrentUserInRole);
        }

        public bool IsCurrentUserAdmin()
        {
            return IsCurrentUserInRole("Admin");
        }

        public bool IsCurrentUserSupport()
        {
            return IsCurrentUserInRole("Support");
        }
    }
}