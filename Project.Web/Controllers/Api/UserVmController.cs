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
        [Authorize(Roles="Admin,Support")]
        public HttpResponseMessage GetPage(int pageNumber, int pageSize, string userId)
        {
            return this.GetPageInner(pageNumber, pageSize, userId);
        }

        [HttpGet]
        public HttpResponseMessage GetPage(int pageNumber, int pageSize)
        {
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            return this.GetPageInner(pageNumber, pageSize, userId);
        }

        private HttpResponseMessage GetPageInner(int pageNumber, int pageSize, string userId)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                this.ModelState.AddModelError("", "PageNumber and PageSize must be grater than 1");
            }
            else
            {
                
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
