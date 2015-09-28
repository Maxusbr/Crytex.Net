using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace Crytex.Notification.Service
{
    public interface INotifyProvider
    {
        string GetUserId(HubCallerContext context);
        bool IsAuth(HubCallerContext context);
        ApplicationUser GetCurrentUser(HubCallerContext context);
        IEnumerable<string> GetRolesForCurrentUser(HubCallerContext context);
        bool IsCurrentUserInRole(HubCallerContext context,string roleName);
        bool IsCurrentUserInAnyRole(HubCallerContext context, List<string> roleName);
        bool IsCurrentUserAdmin(HubCallerContext context);
        bool IsCurrentUserSupport(HubCallerContext context);
    }
}
