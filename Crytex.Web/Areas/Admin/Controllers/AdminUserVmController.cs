using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Web.Models.JsonModels;
using Crytex.Service.IService;
using Crytex.Web.Areas.Admin;


namespace Crytex.Web.Areas.Admin
{
    public class AdminUserVmController : AdminCrytexController
    {
        private readonly IUserVmService _userVmService;

        public AdminUserVmController(IUserVmService userVmService)
        {
            this._userVmService = userVmService;
        }

        /// <summary>
        /// Получения списка машин
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<UserVmViewModel>))]
        [Authorize]
        public IHttpActionResult Get(int pageNumber, int pageSize, string userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = CrytexContext.UserInfoProvider.GetUserId();
            }

            return this.GetPageInner(pageNumber, pageSize, userId);
        }

        /// <summary>
        /// Получение машины по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(UserVmViewModel))]
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var vm = this._userVmService.GetVmById(guid);
            var model = AutoMapper.Mapper.Map<UserVmViewModel>(vm);

            return Ok(model);
        }

        private IHttpActionResult GetPageInner(int pageNumber, int pageSize, string userId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            if (string.IsNullOrEmpty(userId))
            {
                userId = CrytexContext.UserInfoProvider.GetUserId();
            }
            var page = this._userVmService.GetPage(pageNumber, pageSize, userId);
            var viewModel = AutoMapper.Mapper.Map<PageModel<UserVmViewModel>>(page);
            return Ok(viewModel);
        }
    }
}
