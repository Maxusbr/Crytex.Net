using System.Collections.Generic;
using System.Linq;
using Project.Core;
using Project.Data.IRepository;
using Project.Model.Enums;
using Project.Model.Models.Notifications;
using Project.Service.IService;

namespace Project.Service.Service
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private IEmailTemplateRepository _emailTemplateRepository { get; }

        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        public EmailTemplate AddTemplate(string subject, string body, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> parameters = null)
        {
            var newTemplate = new EmailTemplate()
            {
                Subject = subject,
                Body = body,
                EmailTemplateTypeEnum = emailTemplateType,
                ParameterNamesList = parameters
            };
            _emailTemplateRepository.Add(newTemplate);
            _emailTemplateRepository.SaveChanges();
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
            _emailTemplateRepository.SaveChanges();
            LoggerCrytex.Logger.Info("Email Template with id " + id + " and type " + template.EmailTemplateType + " was updated");

            return template;
        }

        public EmailTemplate GetTemplateByType(EmailTemplateType emailTemplateType)
        {
            var templates = _emailTemplateRepository.GetMany(x => x.EmailTemplateType == (int)emailTemplateType).ToList();

            if (!templates.Any())
                LoggerCrytex.Logger.Error("Attempt to get a non-existent email template: " + emailTemplateType);

            return templates.First();
        }

        public void DeleteTemplate(int id)
        {
            var template = _emailTemplateRepository.GetById(id);
            _emailTemplateRepository.Delete(template);
            _emailTemplateRepository.SaveChanges();
            LoggerCrytex.Logger.Warn("Email Template with id " + id + " and type " + template.EmailTemplateType + " was deleted from DB.");
        }

        public void DeleteTemplate(EmailTemplateType emailTemplateType)
        {
            _emailTemplateRepository.Delete(x => x.EmailTemplateType == (int)emailTemplateType);
            _emailTemplateRepository.SaveChanges();
            LoggerCrytex.Logger.Warn("Email Templates with type " + emailTemplateType + " were deleted from DB.");

        }

        public List<EmailTemplate> GetTemplateByTypes(List<EmailTemplateType> templateTypes)
        {
            var templateIntTypes = templateTypes.Select(x => (int)x).ToList();
            return _emailTemplateRepository.GetMany(x => templateIntTypes.Contains(x.EmailTemplateType)).ToList();
        }
    }
}