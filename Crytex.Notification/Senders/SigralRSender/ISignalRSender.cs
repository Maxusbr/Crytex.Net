using System;
using System.Collections.Generic;
using Crytex.Model.Models.Notifications;

namespace Crytex.Notification
{
    public interface ISignalRSender
    {
        void Send(BaseNotify message);
        void Sybscribe(string vmId);
        void SendVmMessage(Guid vmId, MonitorHub.StateMachine stateMachine);
        List<Guid> GetVMs();
    }
}
