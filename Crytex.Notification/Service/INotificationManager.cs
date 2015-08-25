namespace Crytex.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Project.Model.Enums;

    public interface INotificationManager {
        void SetEmailInQueue(string from, string to, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null);

        Task SendEmailImmediately(string @from, string to, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null);
    }
}