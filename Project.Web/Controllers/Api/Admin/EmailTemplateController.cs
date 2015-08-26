using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Project.Model.Enums;
using Project.Model.Models.Notifications;
using Project.Service.IService;
using Project.Web.Models.JsonModels;

namespace Project.Web.Controllers.Api.Admin
{
    public class EmailTemplateController : ApiController
    {
        private IEmailTemplateService _emailTemplateService { get; set; }

        public EmailTemplateController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }


        // GET api/EmailTemplate
        public JsonResult<List<EmailTemplateViewModel>> Get()
        {
            var emailTemplates = _emailTemplateService.GetAllTemplates();
            var model = AutoMapper.Mapper.Map<List<EmailTemplate>, List<EmailTemplateViewModel>>(emailTemplates);
            return Json(model);
        }

        // GET api/EmailTemplate/5
        public HttpResponseMessage Get(int id)
        {
            var emailTemplate = _emailTemplateService.GetTemplateById(id);
            if (emailTemplate == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Email Tepmlate with id = '" + id + "' is not found.");

            var model = AutoMapper.Mapper.Map<EmailTemplate, EmailTemplateViewModel>(emailTemplate);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        

        // POST api/EmailTemplate
        public HttpResponseMessage Post([FromBody]EmailTemplateViewModel model)
        {
            if (!ModelState.IsValid || model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            //когда ParameterNames введены в неправильном формате
            if(!string.IsNullOrEmpty(model.ParameterNames) && !model.ParameterNamesList.Any())
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ParameterNames has not valid format");

            //когда Body и Subject содержат не все переменные из ParameterNames
            var emailText = (model.Subject + model.Body);
            if (!model.ParameterNamesList.TrueForAll(x => emailText.IndexOf("{" + x.Key + "}", StringComparison.Ordinal) >= 0))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Subject and Body contain not all properties that are noticed in ParameterNames.");

            var templates = _emailTemplateService.GetTemplateByType(model.EmailTemplateType);
            if (templates != null)
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Email Template with such Type already exists.");

            var emailTemplate = _emailTemplateService.AddTemplate(model.Subject, model.Body, model.EmailTemplateType, model.ParameterNamesList);
            return Request.CreateResponse(HttpStatusCode.Created, new { id = emailTemplate.Id });
        }

        // PUT api/EmailTemplate/5
        public HttpResponseMessage Put(int id, [FromBody]UpdateEmailTemplateViewModel model)
        {
            if (!ModelState.IsValid || model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var emailText = model.Subject + model.Body;
            if (!model.ParameterNamesList.TrueForAll(x => emailText.IndexOf("{" + x.Key + "}", StringComparison.Ordinal) >= 0))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Subject and Body contain not all properties that are noticed in ParameterNames.");

            var template = _emailTemplateService.GetTemplateById(id);

            if (template == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Email Tepmlate with id = '"+id+"' is not found.");

            _emailTemplateService.UpdateTemplate(id, model.Subject, model.Body, model.ParameterNamesList);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/EmailTemplate/5
        public void Delete(int id)
        {
            _emailTemplateService.DeleteTemplate(id);
        }

        // удаляет все шаблоны в БД заданного типа
        // DELETE api/EmailTemplate?type=ChangeProfile
        // DELETE api/EmailTemplate?type=0
        public void Delete(EmailTemplateType type)
        {
            _emailTemplateService.DeleteTemplate(type);
        }
    }
}