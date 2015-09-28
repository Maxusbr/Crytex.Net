using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Notification.Models;

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

        public void Sybscribe(string vmId)
        {
            throw new NotImplementedException();
        }

        public void SendVmMessage(Guid vmId, StateMachine stateMachine)
        {
            throw new NotImplementedException();
        }

        public List<Guid> GetVMs()
        {
            throw new NotImplementedException();
        }

        public void SendToUserNotification(object message)
        {
            throw new NotImplementedException();
        }

        public void SendToUserNotification(string userId, object message)
        {
            throw new NotImplementedException();
        }
    }
}
