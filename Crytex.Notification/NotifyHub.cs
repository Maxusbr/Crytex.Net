﻿using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Crytex.Notification.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR.Hubs;


namespace Crytex.Notification
{
    public class NotifyHub : CrytexHub, INotifyHub
    {
        private readonly static ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();

        public void Notify(BaseNotify message)
        {
            this.Clients.All.notify(message);
        }

        public override Task OnConnected()
        {
            string userId = CrytexContext.UserInfoProvider.GetUserId();

            Connections.Add(userId, Context.ConnectionId);

            return base.OnConnected();
        }

        public void SendToUser(string userId, Object message, string nameMethod)
        {
            foreach (var userConnection in Connections.GetUserConnections(userId))
            {
                IClientProxy proxy = Clients.Client(userConnection.ConnectionId);
                proxy.Invoke(nameMethod, message);
            }
        }

        public void SendToUserNotification(Object message)
        {
            string userId = CrytexContext.UserInfoProvider.GetUserId();
            SendToUser(userId, message, "newNotification");
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userId = CrytexContext.UserInfoProvider.GetUserId();

            Connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string userId = CrytexContext.UserInfoProvider.GetUserId();
            var searchConnection = Connections.GetUserConnections(userId).FirstOrDefault(c=>c.ConnectionId == Context.ConnectionId);
            if (searchConnection == null)
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }


}