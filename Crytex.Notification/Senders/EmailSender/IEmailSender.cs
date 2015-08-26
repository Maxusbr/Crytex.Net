using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Models;
using Project.Model.Models.Notifications;

namespace Crytex.Notification
{  
    public interface IEmailSender
    {
        Task<KeyValuePair<int, EmailResult>> SendEmail(EmailInfo emailInfo);
        Task<List<KeyValuePair<int, EmailResult>>> SendEmails(List<EmailInfo> emailMessages);
    }
}
