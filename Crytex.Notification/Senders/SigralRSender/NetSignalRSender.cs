using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Flurl.Util;

namespace Crytex.Notification.Senders.SigralRSender
{
    public class NetSignalRSender : ISignalRSender
    {
        private Dictionary<HubNames, Lazy<IHubProxy>> _hubProxy = new Dictionary<HubNames, Lazy<IHubProxy>>();
        private string _hubUrl;

        public enum HubNames
        {
            NotifyHub,
            MonitorHub
        }

        public NetSignalRSender(string hubUrl)
        {
            this._hubUrl = hubUrl;
           
            _hubProxy.Add(HubNames.NotifyHub, new Lazy<IHubProxy>(()=>InitProxy("NotifyHub")));
            _hubProxy.Add(HubNames.MonitorHub, new Lazy<IHubProxy>(() => InitProxy("MonitorHub")));
        }

        private IHubProxy InitProxy(string nameProxy)
        {
            var hubConnection = new HubConnection(this._hubUrl);
            var proxy = hubConnection.CreateHubProxy(nameProxy);
            hubConnection.Start().Wait();

            return proxy;
        }

        public void Send(BaseNotify message)
        {
            Lazy<IHubProxy> hubProxy;
            if (_hubProxy.TryGetValue(HubNames.NotifyHub, out hubProxy))
            {
                hubProxy.Value.Invoke("Notify", message);
            }
        }

        public void Sybscribe(string vmId)
        {
            Lazy<IHubProxy> hubProxy;
            if (_hubProxy.TryGetValue(HubNames.MonitorHub, out hubProxy))
            {
                hubProxy.Value.Invoke("Sybscribe", vmId);
            }
        }

        public void SendVmMessage(Guid vmId, StateMachine stateMachine)
        {
            Lazy<IHubProxy> hubProxy;
            if (_hubProxy.TryGetValue(HubNames.MonitorHub, out hubProxy))
            {
                hubProxy.Value.Invoke("SendVmMessage", vmId, stateMachine);
            }
        }

        public List<Guid> GetVMs()
        {
            throw new NotImplementedException();
        }
    }
}
