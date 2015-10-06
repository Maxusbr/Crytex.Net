using Crytex.Model.Models;
using Crytex.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;
using Crytex.Web.Service;
using Microsoft.Practices.Unity;

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
        public IHttpActionResult Get()
        {
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var servers = this._serverTemplateService.GeAllForUser(userId).ToList();
            var model = AutoMapper.Mapper.Map<List<ServerTemplate>, List<ServerTemplateViewModel>>(servers);

            return Ok(model);
        }

        // GET: api/ServerTemplate/5
        public IHttpActionResult Get(int id)
        {
            var os = this._serverTemplateService.GeById(id);
            var model = AutoMapper.Mapper.Map<ServerTemplateViewModel>(os);

            return Ok(model);
        }

        // POST: api/ServerTemplate
        public IHttpActionResult Post([FromBody]ServerTemplateEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTemplate = AutoMapper.Mapper.Map<ServerTemplate>(model);
            newTemplate = this._serverTemplateService.CreateTemplate(newTemplate);

            return Ok(new { id = newTemplate.Id });
        }

        // PUT: api/ServerTemplate/5
        public IHttpActionResult Put(int id, [FromBody]ServerTemplateEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTemplate = AutoMapper.Mapper.Map<ServerTemplate>(model);
            this._serverTemplateService.Update(id, updatedTemplate);

            return Ok();
        }

        // DELETE: api/ServerTemplate/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            this._serverTemplateService.DeleteById(id);
            return Ok();
        }
    }
}
