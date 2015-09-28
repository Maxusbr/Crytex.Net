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
    public abstract class CrytexHub : Hub, IUserInfoProvider
    {
        [Dependency]
        public IServerConfig ServerConfig { get; }
        [Dependency]
        public INotifyProvider NotifyProvider { get; set; }
        public string GetUserId()
        {
            return NotifyProvider.GetUserId(Context);
        }

        public bool IsAuth()
        {
            return NotifyProvider.IsAuth(Context);
        }


        public ApplicationUser GetCurrentUser()
        {
            return NotifyProvider.GetCurrentUser(Context);
        }


        public IEnumerable<string> GetRolesForCurrentUser()
        {
            return NotifyProvider.GetRolesForCurrentUser(Context);
        }


        public bool IsCurrentUserInRole(string roleName)
        {
            return NotifyProvider.IsCurrentUserInRole(Context, roleName);
        }

        public bool IsCurrentUserInAnyRole(List<string> roleName)
        {
            return NotifyProvider.IsCurrentUserInAnyRole(Context, roleName);
        }

        public bool IsCurrentUserAdmin()
        {
            return NotifyProvider.IsCurrentUserAdmin(Context);
        }

        public bool IsCurrentUserSupport()
        {
            return NotifyProvider.IsCurrentUserSupport(Context);
        }
    }
}
