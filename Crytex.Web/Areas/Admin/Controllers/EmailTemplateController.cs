using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Model.Enums;
using Crytex.Model.Models.Notifications;
using Crytex.Service.IService;
using Crytex.Web.Areas.Admin;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin
{
    public class EmailTemplateController : AdminCrytexController
    {
        private IEmailTemplateService _emailTemplateService { get; set; }

        public EmailTemplateController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        // GET api/EmailTemplate
        public IHttpActionResult Get()
        {
            var emailTemplates = _emailTemplateService.GetAllTemplates();
            var model = AutoMapper.Mapper.Map<List<EmailTemplate>, List<EmailTemplateViewModel>>(emailTemplates);
            return Ok(model);
        }

        // GET api/EmailTemplate/5
        public IHttpActionResult Get(int id)
        {
            var emailTemplate = _emailTemplateService.GetTemplateById(id);
            if (emailTemplate == null)
                return NotFound();

            var model = AutoMapper.Mapper.Map<EmailTemplate, EmailTemplateViewModel>(emailTemplate);
            return Ok(model);
        }

        // POST api/EmailTemplate
        public IHttpActionResult Post([FromBody]EmailTemplateViewModel model)
        {
            if (!ModelState.IsValid || model == null)
                return BadRequest(ModelState);

            //когда ParameterNames введены в неправильном формате
            if (!string.IsNullOrEmpty(model.ParameterNames) && !model.ParameterNamesList.Any())
                return BadRequest("ParameterNames has not valid format");

            //когда Body и Subject содержат не все переменные из ParameterNames
            var emailText = (model.Subject + model.Body);
            if (!model.ParameterNamesList.TrueForAll(x => emailText.IndexOf("{" + x.Key + "}", StringComparison.Ordinal) >= 0))
                return BadRequest("Subject and Body contain not all properties that are noticed in ParameterNames.");

            var templates = _emailTemplateService.GetTemplateByType(model.EmailTemplateType);
            if (templates != null)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Email Template with such Type already exists."));

            var emailTemplate = _emailTemplateService.AddTemplate(model.Subject, model.Body, model.EmailTemplateType, model.ParameterNamesList);
            return Ok(new { id = emailTemplate.Id });
        }

        // PUT api/EmailTemplate/5
        public IHttpActionResult Put(int id, [FromBody]UpdateEmailTemplateViewModel model)
        {
            if (!ModelState.IsValid || model == null)
                return BadRequest(ModelState);

            var emailText = model.Subject + model.Body;
            if (!model.ParameterNamesList.TrueForAll(x => emailText.IndexOf("{" + x.Key + "}", StringComparison.Ordinal) >= 0))
                return BadRequest("Subject and Body contain not all properties that are noticed in ParameterNames.");

            var template = _emailTemplateService.GetTemplateById(id);

            if (template == null)
                return NotFound();

            _emailTemplateService.UpdateTemplate(id, model.Subject, model.Body, model.ParameterNamesList);
            return Ok();
        }

        // DELETE api/EmailTemplate/5
        public IHttpActionResult Delete(int id)
        {
            _emailTemplateService.DeleteTemplate(id);
            return Ok();
        }

        // удаляет все шаблоны в БД заданного типа
        // DELETE api/EmailTemplate?type=ChangeProfile
        // DELETE api/EmailTemplate?type=0
        public IHttpActionResult Delete(EmailTemplateType type)
        {
            _emailTemplateService.DeleteTemplate(type);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult GetTypes()
        {
            var model = Enum.GetValues(typeof(EmailTemplateType)).Cast<EmailTemplateType>().Select(x => new KeyValuePair<string, int>(x.ToString(), (int)x)).ToList();
            return Ok(model);
        }
    }
}