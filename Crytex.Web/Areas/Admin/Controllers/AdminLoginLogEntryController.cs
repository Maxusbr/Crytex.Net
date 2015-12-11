using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminLoginLogEntryController : AdminCrytexController
    {
        private readonly IUserLoginLogService _userLoginLogService;

        public AdminLoginLogEntryController(IUserLoginLogService userLoginLogService)
        {
            this._userLoginLogService = userLoginLogService;
        }

        public IHttpActionResult Get(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null, string userId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var logEntries = this._userLoginLogService.GetPage(pageNumber, pageSize, to, from, userId);
            var pageModel = AutoMapper.Mapper.Map<PageModel<UserLoginLogEntryModel>>(logEntries);

            return this.Ok(pageModel);
        }
    }
}