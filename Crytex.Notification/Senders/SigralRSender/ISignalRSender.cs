using Crytex.Model.Models.Notifications;

namespace Crytex.Notification
{
    public interface ISignalRSender
    {
        void Send(BaseNotify message);
    }
}
