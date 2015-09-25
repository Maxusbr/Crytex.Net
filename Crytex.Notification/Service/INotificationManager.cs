using Crytex.Model.Models;

namespace Crytex.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Crytex.Model.Enums;

    public interface INotificationManager {
        void SetEmailInQueue(string from, string to, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null);

        Task SendEmailImmediately(string @from, string to, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null);

        Task HandleQueueInDB();

        void Sybscribe(string vmId);
        void SendVmMessage(Guid vmId, StateMachine stateMachine);
        List<Guid> GetVMs();
    }
}