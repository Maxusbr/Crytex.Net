using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Models;
using Project.Model.Models.Notifications;

namespace Crytex.Notification
{  
    public interface IEmailSender
    {
        Task<EmailResult> SendEmail(EmailInfo emailInfo);
        Task<List<EmailResult>> SendEmails(List<EmailInfo> emailMessages);
    }
}
