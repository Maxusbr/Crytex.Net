using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;
using Crytex.Service.IService;

namespace Crytex.Web.Controllers.Api
{
    public class UserVmController : CrytexApiController
    {
        private readonly IUserVmService _userVmService;

        public UserVmController(IUserVmService userVmService)
        {
            this._userVmService = userVmService;
        }

        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetPage(int pageNumber, int pageSize, string userId = null)
        {
            if (userId == null)
            {
                userId = this.CrytexContext.UserInfoProvider.GetUserId();
            }
            else if (!(this.CrytexContext.UserInfoProvider.IsCurrentUserInRole("Admin") ||
                this.CrytexContext.UserInfoProvider.IsCurrentUserInRole("Support")))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Only Admin and Support user can acces other user Vm info");
            }
            return this.GetPageInner(pageNumber, pageSize, userId);
        }

        private HttpResponseMessage GetPageInner(int pageNumber, int pageSize, string userId)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                this.ModelState.AddModelError("", "PageNumber and PageSize must be grater than 1");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var page = this._userVmService.GetPage(pageNumber, pageSize, userId);
            var viewModel = AutoMapper.Mapper.Map<PageModel<UserVmViewModel>>(page);
            return Request.CreateResponse(HttpStatusCode.OK, viewModel);
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
