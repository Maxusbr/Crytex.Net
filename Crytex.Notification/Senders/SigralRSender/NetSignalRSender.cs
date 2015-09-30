using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Core.AppConfig;
using Crytex.Model.Models;
using Crytex.Notification.Models;
using Flurl.Util;

namespace Crytex.Notification.Senders.SigralRSender
{
    public class NetSignalRSender : ISignalRSender
    {
        private Dictionary<HubNames, Lazy<IHubProxy>> _hubProxy = new Dictionary<HubNames, Lazy<IHubProxy>>();
        private IAppConfig _appConfig;

        public enum HubNames
        {
            NotifyHub,
            MonitorHub
        }

        public NetSignalRSender(IAppConfig appConfig)
        {
            this._appConfig = appConfig;
           
            _hubProxy.Add(HubNames.NotifyHub, new Lazy<IHubProxy>(()=>InitProxy("NotifyHub")));
            _hubProxy.Add(HubNames.MonitorHub, new Lazy<IHubProxy>(() => InitProxy("MonitorHub")));
        }

        private IHubProxy InitProxy(string nameProxy)
        {
            var hubConnection = new HubConnection(_appConfig.GetValue("HUB_URL"));
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
            Lazy<IHubProxy> hubProxy;
            if (_hubProxy.TryGetValue(HubNames.MonitorHub, out hubProxy))
            {
                return hubProxy.Value.Invoke<List<Guid>>("GetVMs").Result;
            }
            return new List<Guid>();
        }

        public void SendToUserNotification(object message)
        {
            throw new NotImplementedException();
        }

        public void SendToUserNotification(string userId, object message)
        {
            Lazy<IHubProxy> hubProxy;
            if (_hubProxy.TryGetValue(HubNames.NotifyHub, out hubProxy))
            {
                hubProxy.Value.Invoke("SendToUserNotification", userId, message);
            }
        }
    }
}
