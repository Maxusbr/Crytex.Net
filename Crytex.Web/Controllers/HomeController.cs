using Crytex.Model.Models.Notifications;
using Crytex.Notification;
using System.Web.Mvc;
using Crytex.Notification.Models;

namespace Crytex.Web.Controllers
{
    public class HomeController : Controller
    {
        private ISignalRSender _signalRSender;

        public HomeController(ISignalRSender signalRSender)
        {
            this._signalRSender = signalRSender;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SignalRTest()
        {
            return View();
        }

        public void SendSignalR(BaseNotify message)
        {
            this._signalRSender.Send(message);
        }
    }
}