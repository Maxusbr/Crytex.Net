namespace Project.Service.IService
{
    using System;
    using System.Collections.Generic;
    using Project.Model.Enums;
    using Project.Model.Models.Notifications;

    public interface IEmailInfoService
    {
        EmailInfo GetEmail(int id);

        List<EmailInfo> GetEmailsByEmail(string toEmail);

        EmailInfo SaveEmail(string @from, string to, EmailTemplateType emailTemplateType, bool isSentImmediately, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null);

        void MarkEmailAsSent(int id, EmailResultStatus emailResultStatus, string reason = null);

        void DeleteEmail(int id);

        List<EmailInfo> GetEmailInQueue();
    }
}
