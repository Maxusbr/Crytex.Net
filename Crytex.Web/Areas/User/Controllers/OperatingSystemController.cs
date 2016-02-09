using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.IService;
using Crytex.Web.Areas.User;
using Crytex.Web.Models.JsonModels;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Web.Areas.Admin
{
    [AllowAnonymous]
    public class OperatingSystemController : UserCrytexController
    {
        private readonly IOperatingSystemsService _oparaingSystemsService;
        private readonly ApplicationUserManager _userManager;

        public OperatingSystemController(IOperatingSystemsService operatingSystemsService)
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


    }
}
