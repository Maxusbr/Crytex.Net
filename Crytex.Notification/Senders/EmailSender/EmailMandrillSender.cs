using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Mandrill.Requests.Templates;
using Project.Model.Models.Notifications;
using Project.Service.IService;

namespace Crytex.Notification
{    
    public class EmailMandrillSender : IEmailSender
    {
        MandrillApi _mandrillApi { get; }

        IEmailTemplateService _emailTemplateService { get; }


        public EmailMandrillSender(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
            _mandrillApi = new MandrillApi(ConfigurationManager.AppSettings["MandrillApiKey"]);
        }

        public async Task<EmailResult> SendEmail(EmailInfo emailInfo)
        {
            var resultLIst = await SendEmails(new List<EmailInfo> { emailInfo });
            return resultLIst.First();
        }

        public async Task<List<EmailResult>> SendEmails(List<EmailInfo> emailMessages)
        {
            var templateTypes = emailMessages.GroupBy(x => x.EmailTemplateTypeEnum).Select(x => x.Key).ToList();
            var templates = _emailTemplateService.GetTemplateByTypes(templateTypes);

            var resultList = new List<EmailResult>();
            foreach (var emailMessage in emailMessages)
            {
                var emailTemplate = templates.First(x => x.EmailTemplateType == emailMessage.EmailTemplateType);
                var resultSending = await ExecuteSendingEmail(emailMessage, emailTemplate);
                resultList.AddRange(resultSending);
            }
            return resultList;
        }

        #region Helpers

        private async Task<List<EmailResult>> ExecuteSendingEmail(EmailInfo emailInfo, EmailTemplate emailTemplate)
        {
            var templateName = await GetTemplateNameAndAddIfNotExists(emailTemplate);

            var email = new EmailMessage
            {
                FromEmail = emailInfo.From,
                FromName = "Crytex",
                To = new[] { new EmailAddress(emailInfo.To) },
                Subject = MandrillUtil.GenerateTextFromTemplate(emailTemplate.Subject, emailInfo.SubjectParamsList),

            };

            AddUserDynamicContent(email, emailInfo.To, emailInfo.BodyParamsList);

            var request = new SendMessageTemplateRequest(email, templateName, null);

            if (emailInfo.DateSending != null)
                request.SendAt = emailInfo.DateSending;

            List<EmailResult> result = await _mandrillApi.SendMessageTemplate(request);
            return result;
        }

        private void AddUserDynamicContent(EmailMessage emailMessage, string to, List<KeyValuePair<string, string>> parameters)
        {
            for (int j = 0; j < parameters.Count; j++)
            {
                emailMessage.AddRecipientVariable(to, MandrillUtil.GetMergeTag(parameters.ElementAt(j).Key), parameters.ElementAt(j).Value);
            }
        }

        private async Task<string> GetTemplateNameAndAddIfNotExists(EmailTemplate template)
        {
            var templateName = MandrillUtil.GenerateName(template);

            var templateExists = await TemplateExists(templateName);
            if (!templateExists)
            {
                await _mandrillApi.AddTemplate(new AddTemplateRequest(templateName)
                {
                    Code = MandrillUtil.FormatBody(template.Body),
                    Text = string.Empty,
                    Publish = true,
                });
            }

            return templateName;
        }

        private async Task<bool> TemplateExists(string templateName)
        {
            //TODO: избавиться от частого обращения к почтовому сервису для проверки наличия такого шаблона
            TemplateInfo template = null;

            try
            {
                template = await _mandrillApi.TemplateInfo(new TemplateInfoRequest(templateName));
            }
            catch { }

            return template != null;
        }

        #endregion

    }
}