using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Controllers.Api
{
    public class ServerTemplateController : CrytexApiController
    {
        private readonly IServerTemplateService _serverTemplateService;

        public ServerTemplateController(IServerTemplateService serverTemplateService)
        {
            this._serverTemplateService = serverTemplateService;
        }

        // GET: api/ServerTemplate
        [HttpGet]
        public IEnumerable<ServerTemplateViewModel> GetAllForUser()
        {
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var servers = this._serverTemplateService.GeAllForUser(userId).ToList();
            var model = AutoMapper.Mapper.Map<List<ServerTemplate>, List<ServerTemplateViewModel>>(servers);

            return model;
        }

        // GET: api/ServerTemplate/5
        public ServerTemplateViewModel GetById(int id)
        {
            var os = this._serverTemplateService.GeById(id);
            var model = AutoMapper.Mapper.Map<ServerTemplateViewModel>(os);

            return model;
        }

        // POST: api/ServerTemplate
        [HttpPost]
        public HttpResponseMessage Create(ServerTemplateEditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var newTemplate = AutoMapper.Mapper.Map<ServerTemplate>(model);
                newTemplate = this._serverTemplateService.CreateTemplate(newTemplate);

                return Request.CreateResponse(HttpStatusCode.Created, new { id = newTemplate.Id });
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        // PUT: api/ServerTemplate/5
        [HttpPut]
        public HttpResponseMessage Update(int id, ServerTemplateEditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var updatedTemplate = AutoMapper.Mapper.Map<ServerTemplate>(model);
                this._serverTemplateService.Update(id, updatedTemplate);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        // DELETE: api/ServerTemplate/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            this._serverTemplateService.DeleteById(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
