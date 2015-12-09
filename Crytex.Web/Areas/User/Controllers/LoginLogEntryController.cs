using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.User.Controllers
{
    public class LoginLogEntryController : UserCrytexController
    {
        private readonly IUserLoginLogService _userLoginLogService;

        public LoginLogEntryController(IUserLoginLogService userLoginLogService)
        {
            this._userLoginLogService = userLoginLogService;
        }

        public IHttpActionResult Get(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var userId = this.CrytexContext.UserInfoProvider.GetUserId();

            var page = this._userLoginLogService.GetPage(pageNumber, pageSize, from, to, userId);
            var pageModel = AutoMapper.Mapper.Map<PageModel<UserLoginLogEntryModel>>(page);

            return this.Ok(pageModel);
        }
    }
}