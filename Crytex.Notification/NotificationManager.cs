using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Service.IService;
using Project.Model.Models.Notifications;
using Crytex.Notification.NotificationBuilders;

namespace Crytex.Notification
{
    public class NotificationManager
    {
        private ISignalRSender _signalRSender;
        private IEmailSender _emailSender;
        private INotificationService _notificationService;

        private IEmailNotificationBuilder _emailNotificationBuilder;
        private ISignalRNotificationBuilder _signalRNotificationBuilder;

        public NotificationManager(ISignalRSender signalRSender, IEmailSender emailSender,
            INotificationService notificationService, IEmailNotificationBuilder emailNotificationBuilder,
            ISignalRNotificationBuilder signalRNotificationBuilder)
        {
            this._signalRSender = signalRSender;
            this._emailSender = emailSender;
            this._notificationService = notificationService;
            this._emailNotificationBuilder = emailNotificationBuilder;
            this._signalRNotificationBuilder = signalRNotificationBuilder;
        }

        public void SendNotification(ExampleNotification notification)
        {
            var email = this._emailNotificationBuilder.Build(notification);
            this._emailSender.Send(email);
        }
    }
}
