using System;
using Project.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project.Model.Enums;
using System.Threading.Tasks;
using Mandrill.Models;
using NLog;
using Project.Core;
using Project.Model.Models.Notifications;
using EmailResultStatus = Project.Model.Enums.EmailResultStatus;

namespace Crytex.Notification
{
    public class NotificationManager : INotificationManager
    {
        private IEmailSender _emailSender { get; }
        private IEmailInfoService _emailInfoService { get; }

        public NotificationManager(IEmailSender emailSender,
            IEmailInfoService emailInfoService)
        {
            _emailSender = emailSender;
            _emailInfoService = emailInfoService;

        }

        public void SetEmailInQueue(string @from, string to, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null)
        {
            _emailInfoService.SaveEmail(from, to, emailTemplateType, false, subjectParams, bodyParams, dateSending);
        }

        public async Task SendEmailImmediately(string @from, string to, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null)
        {
            var email = _emailInfoService.SaveEmail(@from, to, emailTemplateType, true, subjectParams, bodyParams, dateSending);
            var resultSending = await _emailSender.SendEmail(email);
            _emailInfoService.MarkEmailAsSent(email.Id, (EmailResultStatus)resultSending.Value.Status, resultSending.Value.RejectReason);
        }

        public async Task HandleQueueInDB()
        {
            var emails = _emailInfoService.GetEmailInQueue();
            var resultSendings = await _emailSender.SendEmails(emails);

            if (emails.Any())
                LogResultHandlingEmailQueue(emails, resultSendings);

            foreach (var resultSending in resultSendings)
                _emailInfoService.MarkEmailAsSent(resultSending.Key, (EmailResultStatus)resultSending.Value.Status, resultSending.Value.RejectReason);
        }

        static void LogResultHandlingEmailQueue(List<EmailInfo> emails, List<KeyValuePair<int, EmailResult>> resultSendings)
        {
            var report = new StringBuilder();
            report.AppendLine("EmailSendJob has worked sending Emails:");
            report.AppendLine("Total Email in Queue: " + emails.Count);
            foreach (var result in Enum.GetValues(typeof(Mandrill.Models.EmailResultStatus)).Cast<Mandrill.Models.EmailResultStatus>())
                report.AppendLine(result + " emails: " + resultSendings.Count(x => x.Value.Status == result));
            report.AppendLine("Not sent emails: " + (emails.Count - resultSendings.Count));
            LoggerCrytex.Logger.Info(report);
        }
    }
}
