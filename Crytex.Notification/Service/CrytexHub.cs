using Crytex.Core.Service;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Microsoft.AspNet.Identity;


namespace Crytex.Notification.Service
{
    public abstract class CrytexHub : Hub
    {
        [Dependency]
        public ICrytexContext CrytexContext { get; set; }
        //[Dependency]
        //public ApplicationUserManager UserManager { get; set; }
    }
}
