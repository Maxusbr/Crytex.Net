using System.Collections.Generic;
using System.Linq;
using Project.Core;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Enums;
using Project.Model.Models.Notifications;
using Project.Service.IService;

namespace Project.Service.Service
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private IEmailTemplateRepository _emailTemplateRepository { get; }
        private IUnitOfWork _unitOfWork { get; set; }

        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IUnitOfWork unitOfWork)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _unitOfWork = unitOfWork;
        }

        public EmailTemplate AddTemplate(string subject, string body, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> parameters = null)
        {
            var newTemplate = new EmailTemplate()
            {
                Subject = subject,
                Body = body,
                EmailTemplateType = emailTemplateType,
                ParameterNamesList = parameters
            };
            _emailTemplateRepository.Add(newTemplate);
            _unitOfWork.Commit();
            LoggerCrytex.Logger.Info("Email Template with type " + emailTemplateType + " was added in DB");

            return newTemplate;
        }

        public EmailTemplate UpdateTemplate(int id, string subject, string body, List<KeyValuePair<string, string>> parameters = null)
        {
            var template = _emailTemplateRepository.GetById(id);
            if (template == null)
                return null;

            template.Body = body;
            template.Subject = subject;
            template.ParameterNamesList = parameters;

            _emailTemplateRepository.Update(template);
            _unitOfWork.Commit();
            LoggerCrytex.Logger.Info("Email Template with id " + id + " and type " + template.EmailTemplateType + " was updated");

            return template;
        }

        public EmailTemplate GetTemplateByType(EmailTemplateType emailTemplateType)
        {
            var templates = _emailTemplateRepository.GetMany(x => x.EmailTemplateType == emailTemplateType).ToList();

            if (!templates.Any())
                LoggerCrytex.Logger.Error("Attempt to get a non-existent email template: " + emailTemplateType);

            return templates.FirstOrDefault();
        }

        public void DeleteTemplate(int id)
        {
            var template = _emailTemplateRepository.GetById(id);
            _emailTemplateRepository.Delete(template);
            _unitOfWork.Commit();
            LoggerCrytex.Logger.Warn("Email Template with id " + id + " and type " + template.EmailTemplateType + " was deleted from DB.");
        }

        public void DeleteTemplate(EmailTemplateType emailTemplateType)
        {
            _emailTemplateRepository.Delete(x => x.EmailTemplateType == emailTemplateType);
            _unitOfWork.Commit();
            LoggerCrytex.Logger.Warn("Email Templates with type " + emailTemplateType + " were deleted from DB.");
        }

        public List<EmailTemplate> GetTemplateByTypes(List<EmailTemplateType> templateTypes)
        {
            return _emailTemplateRepository.GetMany(x => templateTypes.Contains(x.EmailTemplateType)).ToList();
        }

        public List<EmailTemplate> GetAllTemplates()
        {
            return _emailTemplateRepository.GetAll().ToList();
        }

        public EmailTemplate GetTemplateById(int id)
        {
            return _emailTemplateRepository.GetById(id);
        }
    }
}