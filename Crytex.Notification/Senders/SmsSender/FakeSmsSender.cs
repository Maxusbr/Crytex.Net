using System.Threading.Tasks;

namespace Crytex.Notification
{
    public class FakeSmsSender : ISmsSender
    {
        public Task Send(string phoneNumber, string messageText)
        {
            return Task.FromResult(0);
        }
    }
}
