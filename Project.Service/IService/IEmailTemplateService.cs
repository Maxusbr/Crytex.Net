using System.Collections.Generic;
using Project.Model.Enums;
using Project.Model.Models.Notifications;

namespace Project.Service.IService
{
    public interface IEmailTemplateService
    {
        EmailTemplate AddTemplate(string subject, string body, EmailTemplateType emailTemplateType, List<KeyValuePair<string, string>> parameters = null);

        EmailTemplate UpdateTemplate(int id, string subject, string body, List<KeyValuePair<string, string>> parameters = null);

        EmailTemplate GetTemplateByType(EmailTemplateType emailTemplateType);

        void DeleteTemplate(int id);
        void DeleteTemplate(EmailTemplateType emailTemplateType);

        List<EmailTemplate> GetTemplateByTypes(List<EmailTemplateType> templateTypes);
    }
}