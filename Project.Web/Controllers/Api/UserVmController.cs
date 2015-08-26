using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Web.Models.JsonModels;
using Project.Service.IService;

namespace Project.Web.Controllers.Api
{
    public class UserVmController : ApiController
    {
        private IUserVmService _userVmService;

        public UserVmController(IUserVmService userVmService)
        {
            this._userVmService = userVmService;
        }

        [HttpGet]
        public HttpResponseMessage GetPage(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                this.ModelState.AddModelError("", "PageNumber and PageSize must be grater than 1");
            }
            else
            {
                var page = this._userVmService.GetPage(pageNumber, pageSize);
                var viewModel = AutoMapper.Mapper.Map<PageModel<UserVmViewModel>>(page);
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpGet]
        public UserVmViewModel Get(int id)
        {
            var vm = this._userVmService.GetVmById(id);
            var model = AutoMapper.Mapper.Map<UserVmViewModel>(vm);
            
            return model;
        }
    }
}
