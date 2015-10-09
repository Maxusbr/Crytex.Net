using Crytex.Model.Models;
using Crytex.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Web.Models.JsonModels;
using Crytex.Web.Service;
using Microsoft.Practices.Unity;

namespace Crytex.Web.Areas.Admin
{
    public class AdminTemplateController : AdminCrytexController
    {
        private readonly IServerTemplateService _serverTemplateService;

        public AdminTemplateController(IServerTemplateService serverTemplateService)
        {
            this._serverTemplateService = serverTemplateService;
        }

        /// <summary>
        /// Получение всех ServerTemplate для userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // GET: api/ServerTemplate
        [ResponseType(typeof(List<ServerTemplateViewModel>))]
        public IHttpActionResult Get(string userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = CrytexContext.UserInfoProvider.GetUserId();
            }

            var servers = this._serverTemplateService.GeAllForUser(userId).ToList();
            var model = AutoMapper.Mapper.Map<List<ServerTemplate>, List<ServerTemplateViewModel>>(servers);

            return Ok(model);
        }

        /// <summary>
        /// Получение ServerTemplate по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/ServerTemplate/5
        [ResponseType(typeof(ServerTemplateViewModel))]
        public IHttpActionResult Get(int id)
        {
            var os = this._serverTemplateService.GeById(id);
            var model = AutoMapper.Mapper.Map<ServerTemplateViewModel>(os);

            return Ok(model);
        }

        /// <summary>
        /// Создание нового ServerTemplate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Обновление ServerTemplate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Удаление ServerTemplate по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ServerTemplate/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            this._serverTemplateService.DeleteById(id);
            return Ok();
        }
    }
}
