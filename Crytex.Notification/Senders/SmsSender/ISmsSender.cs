using System.Threading.Tasks;

namespace Crytex.Notification
{
    public interface ISmsSender
    {
        Task Send(string phoneNumber, string messageText);
    }
}
