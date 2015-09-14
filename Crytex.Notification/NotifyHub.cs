using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Notification
{
    public class NotifyHub : Hub, INotifyHub
    {
        public void Notify(BaseNotify message)
        {
            this.Clients.All.notify(message);
        }
    }
}