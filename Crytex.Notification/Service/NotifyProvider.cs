using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Crytex.Core.Service;
using Crytex.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Crytex.Notification.Service
{
    public class NotifyProvider: INotifyProvider
    {
        public UserManager<ApplicationUser> UserManager { get; set; }

        public NotifyProvider(UserManager<ApplicationUser> userManager)
        {
            this.UserManager = userManager;
        }

        public string GetUserId(HubCallerContext context)
        {
            var userId = context.User.Identity.GetUserId();
            return userId;
        }

        public bool IsAuth(HubCallerContext context)
        {
            return context.User.Identity.IsAuthenticated;
        }


        public ApplicationUser GetCurrentUser(HubCallerContext context)
        {
            var user = this.UserManager.FindById(this.GetUserId(context));
            return user;
        }


        public IEnumerable<string> GetRolesForCurrentUser(HubCallerContext context)
        {
            var roles = this.UserManager.GetRoles(this.GetUserId(context));
            return roles;
        }


        public bool IsCurrentUserInRole(HubCallerContext context, string roleName)
        {
            bool isIn = this.UserManager.IsInRole(this.GetUserId(context), roleName);
            return isIn;
        }

        public bool IsCurrentUserInAnyRole(HubCallerContext context, List<string> roleName)
        {
            return roleName.Any((oneRole)=>IsCurrentUserInRole(context, oneRole));
        }

        public bool IsCurrentUserAdmin(HubCallerContext context)
        {
            return IsCurrentUserInRole(context, "Admin");
        }

        public bool IsCurrentUserSupport(HubCallerContext context)
        {
            return IsCurrentUserInRole(context, "Support");
        }
    }
}
