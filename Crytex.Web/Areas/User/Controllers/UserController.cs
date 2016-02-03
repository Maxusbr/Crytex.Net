using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.User.Controllers
{
    public class UserController : UserCrytexController
    {
        private IApplicationUserService _applicationUserService { get; }
        /// <summary>
        /// Получение пользователя по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/Admin/<controller>/5
        [ResponseType(typeof(ApplicationUserViewModel))]
        public IHttpActionResult Get()
        {
            var user = _applicationUserService.GetUserById(CrytexContext.UserInfoProvider.GetUserId());
            var model = AutoMapper.Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
            return Ok(model);
        }
    }
}