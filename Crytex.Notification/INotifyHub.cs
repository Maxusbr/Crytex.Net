using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models.Notifications;

namespace Crytex.Notification
{
    public interface INotifyHub
    {
        void Notify(BaseNotify message);
    }
}
