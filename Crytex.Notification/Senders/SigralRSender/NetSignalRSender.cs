using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Notification.Senders.SigralRSender
{
    public class NetSignalRSender : ISignalRSender
    {
        private Lazy<IHubProxy> _hubProxy;
        private string _hubUrl;

        public NetSignalRSender(string hubUrl)
        {
            this._hubUrl = hubUrl;
            this._hubProxy = new Lazy<IHubProxy>(InitProxy);
        }

        private IHubProxy InitProxy()
        {
            var hubConnection = new HubConnection(this._hubUrl);
            var proxy = hubConnection.CreateHubProxy("NotifyHub");
            hubConnection.Start().Wait();

            return proxy;
        }

        public void Send(BaseNotify message)
        {
            this._hubProxy.Value.Invoke("Notify", message);
        }
    }
}
