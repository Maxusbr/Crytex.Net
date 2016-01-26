using Crytex.Model.Models;

namespace Crytex.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Crytex.Model.Enums;
    using Model.Models.Biling;

    public interface INotificationManager {
        void SetEmailInQueue(string from, string to, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null);

        Task SendEmailImmediately(string @from, string to, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null);
        Task SendEmailUserByTask(string userId, TypeTask typeTask);
        Task HandleQueueInDB();

        void Sybscribe(string vmId);
        void SendVmMessage(Guid vmId, StateMachine stateMachine);
        List<Guid> GetVMs();
        void SendToUserNotification(string userId, Object message);
        void SendMachinePoweredOffEmail(string userId);
        void SendSubscriptionEndWarningEmail(string userId, int daysToEnd);
        void SendSubscriptionDeletionWarningEmail(string userId, int daysToDeletion);
        void SendNewVmCreationEndEmail(string userId, string vmName, string osUserName, string osUserPassword);
        void SendGameServerPoweredOffEmail(string userId);
        void SendGameServerEndWarningEmail(string userId, int daysToEnd);
        void SendGameServerDeletionWarningEmail(string userId, int daysToDeletion);
    }
}