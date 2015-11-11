using Crytex.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Core;
using Crytex.Data.IRepository;
using Crytex.Model.Enums;
using Crytex.Model.Models;
using Crytex.Model.Models.Notifications;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.Service
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
            return _emailInfoRepository.GetMany(x => x.To == toEmail);
        }

        public IPagedList<EmailInfo> GetEmails(int pageNumber, int pageSize, SearchEmailParams searchParams = null)
        {
            var page = new Page(pageNumber, pageSize);
            Expression<Func<EmailInfo, bool>> where = x => true;

            if (searchParams != null)
            {
                if (searchParams.EmailStatus != null)
                {
                    where = where.And(e => e.EmailResultStatus == searchParams.EmailStatus);
                }
                if (searchParams.FromDate != null)
                {
                    where = where.And(e => e.DateSending > searchParams.FromDate);
                }
                if (searchParams.ToDate != null)
                {
                    where = where.And(e => e.DateSending < searchParams.ToDate);
                }
                if (searchParams.Receiver != null)
                {
                    where = where.And(e => e.To == searchParams.Receiver);
                }
                if (searchParams.Sender != null)
                {
                    where = where.And(e => e.From == searchParams.Sender);
                }
                if (searchParams.IsProcessed != null)
                {
                    where = where.And(e => e.IsProcessed == searchParams.IsProcessed);
                }
            }

            var listPage = _emailInfoRepository.GetPage(page, where, x => x.DateSending);
            
            return listPage;
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

        public List<EmailInfo> GetEmailInQueue()
        {
            var emails = _emailInfoRepository.GetMany(x => !x.IsProcessed);
            return emails;
        }
    }
}