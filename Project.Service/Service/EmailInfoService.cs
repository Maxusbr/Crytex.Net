namespace Project.Service.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Project.Core;
    using Project.Data.IRepository;
    using Project.Model.Enums;
    using Project.Model.Models.Notifications;
    using Project.Service.IService;

    public class EmailInfoService :  IEmailInfoService
    {
        private IEmailInfoRepository _emailInfoRepository { get; }

        public EmailInfoService(IEmailInfoRepository emailInfoRepository)
        {
            _emailInfoRepository = emailInfoRepository;
        }

        public EmailInfo GetEmail(int id)
        {
            return _emailInfoRepository.GetById(id);
        }

        public List<EmailInfo> GetEmailsByEmail(string toEmail)
        {
            return _emailInfoRepository.GetMany(x => x.To == toEmail).ToList();
        }

        public EmailInfo SaveEmail(string @from, string to, EmailTemplateType emailTemplateType, bool isSentImmediately, List<KeyValuePair<string, string>> subjectParams = null, List<KeyValuePair<string, string>> bodyParams = null, DateTime? dateSending = null)
        {
            var newEmail = new EmailInfo()
                           {
                                   From = from,
                                   To = to, 
                                   EmailTemplateTypeEnum =  emailTemplateType,
                                   SubjectParamsList = subjectParams,
                                   BodyParamsList = bodyParams,
                                   DateSending = dateSending
                           };

            if (!isSentImmediately)
                newEmail.EmailResultStatusEnum = EmailResultStatus.Queued;

            _emailInfoRepository.Add(newEmail);
            _emailInfoRepository.SaveChanges();
            return newEmail;
        }

        public void MarkEmailAsSent(int id, EmailResultStatus emailResultStatus, string reason = null)
        {
            var email = _emailInfoRepository.GetById(id);
            email.IsProcessed = true;
            email.EmailResultStatusEnum = emailResultStatus;
            if (reason != null)
                email.Reason = reason;
            email.DateSending = DateTime.UtcNow;

            _emailInfoRepository.Update(email);
            _emailInfoRepository.SaveChanges();
        }

        public void DeleteEmail(int id)
        {
            var email = _emailInfoRepository.GetById(id);
            _emailInfoRepository.Delete(email);
            _emailInfoRepository.SaveChanges();
            LoggerCrytex.Logger.Warn("Email (to: "+ email.To + ", type: "+email.EmailTemplateType+") was deleted");
        }
    }
}