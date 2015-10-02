using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Notification.Models;

namespace Crytex.Notification
{
    public interface INotifyHub
    {
        void Notify(BaseNotify message);
    }
}
