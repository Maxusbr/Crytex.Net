using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Web.Areas.Admin
{
    public class AdminOperatingSystemController : AdminCrytexController
    {
        private readonly IOperatingSystemsService _oparaingSystemsService;
        private readonly ApplicationUserManager _userManager;

        public AdminOperatingSystemController(IOperatingSystemsService operatingSystemsService)
        {
            this._oparaingSystemsService = operatingSystemsService;
        }

        /// <summary>
        /// Получение всех операций системы
        /// </summary>
        /// <returns></returns>
        // GET: api/OperatingSystem
        [ResponseType(typeof(List<OperatingSystemViewModel>))]
        public IHttpActionResult Get()
        {
            var systems = this._oparaingSystemsService.GetAll().ToList();
            var model = AutoMapper.Mapper.Map<List<OperatingSystem>, List<OperatingSystemViewModel>>(systems);

            return Ok(model);
        }

        /// <summary>
        /// Получение операции системы по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/OperatingSystem/5
        [ResponseType(typeof(OperatingSystemViewModel))]
        public IHttpActionResult Get(int id)
        {
            var os = this._oparaingSystemsService.GetById(id);
            var model = AutoMapper.Mapper.Map<OperatingSystemViewModel>(os);

            return Ok(model);
        }

        /// <summary>
        /// Создание операции 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/OperatingSystem
        public IHttpActionResult Post([FromBody]OperatingSystemEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newOS = AutoMapper.Mapper.Map<OperatingSystem>(model);
            newOS = this._oparaingSystemsService.CreateOperatingSystem(newOS);

            return Ok(new { id = newOS.Id });

        }

        /// <summary>
        /// Обновление операции
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT: api/OperatingSystem/5
        public IHttpActionResult Put(int id, [FromBody]OperatingSystemEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var updatedOs = AutoMapper.Mapper.Map<OperatingSystem>(model);
            this._oparaingSystemsService.Update(id, updatedOs);

            return Ok();
        }

        /// <summary>
        /// Удаление операции по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/OperatingSystem/5
        public IHttpActionResult Delete(int id)
        {
            this._oparaingSystemsService.DeleteById(id);
            return Ok();
        }
    }
}
