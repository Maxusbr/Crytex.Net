using Project.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Project.Core;
using Project.Data.IRepository;
using Project.Model.Enums;
using Project.Model.Models.Notifications;
using Project.Service.IService;

namespace Project.Service.Service
{
    public class EmailInfoService :  IEmailInfoService
    {
        private IEmailInfoRepository _emailInfoRepository { get; }
        private IUnitOfWork _unitOfWork { get; set; }

        public EmailInfoService(IEmailInfoRepository emailInfoRepository, IUnitOfWork unitOfWork)
        {
            _emailInfoRepository = emailInfoRepository;
            _unitOfWork = unitOfWork;
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
                                   EmailTemplateType =  emailTemplateType,
                                   SubjectParamsList = subjectParams,
                                   BodyParamsList = bodyParams,
                                   DateSending = dateSending
                           };

            if (!isSentImmediately)
                newEmail.EmailResultStatus = EmailResultStatus.Queued;

            _emailInfoRepository.Add(newEmail);
            _unitOfWork.Commit();
            return newEmail;
        }

        public void MarkEmailAsSent(int id, EmailResultStatus emailResultStatus, string reason = null)
        {
            var email = _emailInfoRepository.GetById(id);
            email.IsProcessed = true;
            email.EmailResultStatus = emailResultStatus;
            if (reason != null)
                email.Reason = reason;
            email.DateSending = DateTime.UtcNow;

            _emailInfoRepository.Update(email);
            _unitOfWork.Commit();
        }

        public void DeleteEmail(int id)
        {
            var email = _emailInfoRepository.GetById(id);
            _emailInfoRepository.Delete(email);
            _unitOfWork.Commit();
            LoggerCrytex.Logger.Warn("Email (to: "+ email.To + ", type: "+email.EmailTemplateType+") was deleted");
        }
    }
}