using System.Collections.Generic;
using Crytex.Model.Enums;
using Crytex.Model.Models.Notifications;

namespace Crytex.Service.IService
{
    public interface IEmailTemplateService
    {
        EmailTemplate AddTemplate(string subject, string body, EmailTemplateType emailTemplateType, List<string> parameters = null);

        EmailTemplate UpdateTemplate(int id, string subject, string body, List<string> parameters = null);

        EmailTemplate GetTemplateByType(EmailTemplateType emailTemplateType);

        void DeleteTemplate(int id);
        void DeleteTemplate(EmailTemplateType emailTemplateType);

        List<EmailTemplate> GetTemplateByTypes(List<EmailTemplateType> templateTypes);

        List<EmailTemplate> GetAllTemplates();

        EmailTemplate GetTemplateById(int id);
    }
}