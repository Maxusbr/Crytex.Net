using Project.Model.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Notification.NotificationBuilders
{
    public interface IEmailNotificationBuilder
    {
        // TODO: change return type to Email
        object Build(ExampleNotification notification);
    }
}
