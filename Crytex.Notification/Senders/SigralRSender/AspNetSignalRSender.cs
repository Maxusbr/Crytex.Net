using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Notification.Senders.SigralRSender
{
    public class AspNetSignalRSender : ISignalRSender
    {
        public void Send(BaseNotify message)
        {
            var connectionManager = GlobalHost.DependencyResolver.Resolve<IConnectionManager>();
            var context = connectionManager.GetHubContext<NotifyHub>();

            context.Clients.All.notify(message);
        }
    }
}
