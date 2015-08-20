using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Notification
{
    public interface IEmailSender
    {
        // TODO: change email type to Email
        void Send(object email);
    }
}
