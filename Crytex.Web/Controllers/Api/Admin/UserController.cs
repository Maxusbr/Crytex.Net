using System.Collections.Generic;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Controllers.Api.Admin
{
    public class UserController : ApiController
    {
        public UserController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        IApplicationUserService _applicationUserService { get; }

        // GET api/<controller>
        public IHttpActionResult Get(int pageSize = 20, int pageIndex = 1, string userName = null, string email = null)
        {
            if (pageIndex <= 0 || pageSize <= 0)
                return BadRequest("PageSize and PageIndex must be positive.");

            var  users = _applicationUserService.GetPage(pageSize, pageIndex, userName,email);
            var model = AutoMapper.Mapper.Map<List<ApplicationUser>, List<ApplicationUserViewModel>>(users);
            return Ok(model);
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id is null or empty.");

            var user = _applicationUserService.GetUserById(id);
            var model = AutoMapper.Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
            return Ok(model);
        }
    }
}