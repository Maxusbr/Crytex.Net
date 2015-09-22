using System;
using System.Collections.Generic;
using Crytex.Model.Models;
using Crytex.Model.Models.Notifications;

namespace Crytex.Notification
{
    public interface ISignalRSender
    {
        void Send(BaseNotify message);
        void Sybscribe(string vmId);
        void SendVmMessage(Guid vmId, StateMachine stateMachine);
        IEnumerable<UserVm> GetVMs();
    }
}
