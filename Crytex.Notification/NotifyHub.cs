using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Crytex.Notification.Service;
using Microsoft.AspNet.Identity;


namespace Crytex.Notification
{
    public class NotifyHub : CrytexHub, INotifyHub
    {
        private readonly Dictionary<string, string> _connections = new Dictionary<string, string>();
        public void Notify(BaseNotify message)
        {
            this.Clients.All.notify(message);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            if (!_connections.ContainsKey(connectionId))
            {
                string userId = CrytexContext.UserInfoProvider.GetUserId();

                _connections.Add(connectionId, userId);
            }

            //return Clients.All.connected(Context.ConnectionId, DateTime.Now.ToString());
            return base.OnConnected();
        }

        public void SendToUser(string userId, Object message)
        {
            var userConnections = _connections.Where(c=>c.Value.Contains(userId));
            foreach (var userConnection in userConnections)
            {
                Clients.Client(userConnection.Key).receiverMessage(message);
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;
            if (_connections.ContainsKey(connectionId))
            {
                _connections.Remove(connectionId);
            }
            //return Clients.All.disconnected(Context.ConnectionId, DateTime.Now.ToString());
            return base.OnDisconnected(stopCalled);
        }
    }
}