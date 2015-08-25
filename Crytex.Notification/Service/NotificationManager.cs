using System;
using Project.Service.IService;
using System.Collections.Generic;
using Project.Model.Enums;
using System.Threading.Tasks;

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
            _emailInfoService.MarkEmailAsSent(email.Id, (EmailResultStatus)resultSending.Status,resultSending.RejectReason);
        }
    }
}
