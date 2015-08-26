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
    public class UserVmController : CrytexApiController
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
                var userId = this.CrytexContext.UserInfoProvider.GetUserId();
                var page = this._userVmService.GetPage(pageNumber, pageSize, userId);
                var viewModel = AutoMapper.Mapper.Map<PageModel<UserVmViewModel>>(page);
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var vm = this._userVmService.GetVmById(guid);
            var model = AutoMapper.Mapper.Map<UserVmViewModel>(vm);

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}
