﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Mandrill.Requests.Templates;
using Crytex.Core;
using Crytex.Model.Models.Notifications;
using Crytex.Service.IService;

namespace Crytex.Notification
{
    public class EmailMandrillSender : IEmailSender
    {
        MandrillApi _mandrillApi { get; }

        IEmailTemplateService _emailTemplateService { get; }


        public EmailMandrillSender(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
            var apiKey = ConfigurationManager.AppSettings["MandrillApiKey"];
            if (apiKey == null)
                throw new ArgumentNullException(nameof(apiKey));

            _mandrillApi = new MandrillApi(apiKey);
        }

        public async Task<KeyValuePair<int, EmailResult>> SendEmail(EmailInfo emailInfo)
        {
            var resultLIst = await SendEmails(new List<EmailInfo> { emailInfo });
            return resultLIst.First();
        }

        public async Task<List<KeyValuePair<int, EmailResult>>> SendEmails(List<EmailInfo> emailMessages)
        {
            var templateTypes = emailMessages.GroupBy(x => x.EmailTemplateType).Select(x => x.Key).ToList();
            var templates = _emailTemplateService.GetTemplateByTypes(templateTypes);

            var resultList = new List<KeyValuePair<int, EmailResult>>();
            foreach (var emailMessage in emailMessages)
            {
                var emailTemplate = templates.First(x => x.EmailTemplateType == emailMessage.EmailTemplateType);
                try
                {
                    var resultSending = await ExecuteSendingEmail(emailMessage, emailTemplate);
                    resultList.Add(new KeyValuePair<int, EmailResult>(emailMessage.Id, resultSending.First()));
                }
                catch (Exception e)
                {
                    LoggerCrytex.Logger.Error(e);
                }
            }
            return resultList;
        }

        #region Helpers

        private async Task<List<EmailResult>> ExecuteSendingEmail(EmailInfo emailInfo, EmailTemplate emailTemplate)
        {
            var templateName = await GetTemplateNameAndAddIfNotExists(emailTemplate);

            CheckEmailParameters(emailInfo, emailTemplate);

            var email = new EmailMessage
            {
                FromEmail = emailInfo.From,
                FromName = "Crytex",
                To = new[] { new EmailAddress(emailInfo.To) },
                Subject = MandrillUtil.GenerateTextFromTemplate(emailTemplate.Subject, emailInfo.SubjectParamsList)
            };

            AddUserDynamicContent(email, emailInfo.To, emailInfo.BodyParamsList);

            var request = new SendMessageTemplateRequest(email, templateName, null);

            //TODO: отправка в определнное время только для платного акка Mandrill
            //if (emailInfo.DateSending != null)
            //    request.SendAt = emailInfo.DateSending;


            List<EmailResult> result = await _mandrillApi.SendMessageTemplate(request);
            return result;
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

        private static void AddUserDynamicContent(EmailMessage emailMessage, string to, List<KeyValuePair<string, string>> parameters)
        {
            for (int j = 0; j < parameters.Count; j++)
            {
                emailMessage.AddRecipientVariable(to, MandrillUtil.GetMergeTag(parameters.ElementAt(j).Key), parameters.ElementAt(j).Value);
            }
        }

        private static void CheckEmailParameters(EmailInfo emailInfo, EmailTemplate emailTemplate)
        {
            var emailParameters = emailInfo.SubjectParamsList.Union(emailInfo.BodyParamsList).Select(x => x.Key).ToList();
            var templateParameters = emailTemplate.ParameterNamesList;

            if (!templateParameters.All(emailParameters.Contains))
                LoggerCrytex.Logger.Error("Email(id: " + emailInfo.Id + ") and Template(id: " + emailTemplate.Id + ", type: " + emailTemplate.EmailTemplateType + ") parameters are not the same.");
        }

        #endregion

    }
}