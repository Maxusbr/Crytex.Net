using System;
using Crytex.Service.IService;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Crytex.Model.Enums;
using System.Threading.Tasks;
using Mandrill.Models;
using Crytex.Core;
using Crytex.Model.Models;
using Crytex.Model.Models.Notifications;
using EmailResultStatus = Crytex.Model.Enums.EmailResultStatus;

namespace Crytex.Notification
{
    public class NotificationManager : INotificationManager
    {
        private IEmailSender _emailSender { get; }
        private IEmailInfoService _emailInfoService { get; }
        private ISignalRSender _signalRSender { get; }
        private IEmailTemplateService _emailTemplateService;
        private IApplicationUserService _applicationUserService;



        public NotificationManager(IEmailSender emailSender,
            IEmailInfoService emailInfoService, 
            ISignalRSender signalRSender,
            IEmailTemplateService emailTemplateService,
            IApplicationUserService applicationUserService)
        {
            _emailSender = emailSender;
            _emailInfoService = emailInfoService;
            _signalRSender = signalRSender;
            _emailTemplateService = emailTemplateService;
            _applicationUserService = applicationUserService;
        }

        public void SendToUserNotification(string userId, Object message)
        {
            _signalRSender.SendToUserNotification(userId, message);
        }

        public void Sybscribe(string vmId)
        {
            _signalRSender.Sybscribe(vmId);
        }

        public void SendVmMessage(Guid vmId, StateMachine stateMachine)
        {
            _signalRSender.SendVmMessage(vmId, stateMachine);
        }

        public List<Guid> GetVMs()
        {
           return _signalRSender.GetVMs();
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

        public async Task SendEmailUserByTask(string userId, TypeTask typeTask)
        {
            EmailTemplateType? emailType = null;

            if (typeTask == TypeTask.UpdateVm)
            {
                emailType = EmailTemplateType.UpdateVm;
            }
            if (typeTask == TypeTask.CreateVm)
            {
                emailType = EmailTemplateType.CreateVm;
            }

            if (emailType != null)
            {
                var template = _emailTemplateService.GetTemplateByType(emailType.Value);
                var user = this._applicationUserService.GetUserById(userId);

                if (template != null)
                {
                    var parameters = template.ParameterNamesList.Select(key => new KeyValuePair<string, string>(key, key)).ToList();
                    var from = ConfigurationManager.AppSettings["Email"];
                    await SendEmailImmediately(from, user.Email, template.EmailTemplateType, parameters, parameters, DateTime.UtcNow);
                }
            }
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
