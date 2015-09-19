using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Crytex.Core.Service;
using Crytex.Model.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Microsoft.AspNet.Identity;


namespace Crytex.Notification.Service
{
    public abstract class CrytexHub : Hub
    {
        [Dependency]
        public IServerConfig ServerConfig { get; }
        [Dependency]
        public UserManager<ApplicationUser> UserManager { get; set; }

        public string GetUserId()
        {
            
            var userId = Context.User.Identity.GetUserId();
            return userId;
        }

        public bool IsAuth()
        {
            return Context.User.Identity.IsAuthenticated;
        }


        public ApplicationUser GetCurrentUser()
        {
            var user = this.UserManager.FindById(GetUserId());
            return user;
        }


        public IEnumerable<string> GetRolesForCurrentUser()
        {
            var roles = this.UserManager.GetRoles(this.GetUserId());
            return roles;
        }


        public bool IsCurrentUserInRole(string roleName)
        {
            bool isIn = this.UserManager.IsInRole(this.GetUserId(), roleName);
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
