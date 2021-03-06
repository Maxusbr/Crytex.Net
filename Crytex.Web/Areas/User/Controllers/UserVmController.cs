﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Web.Models.JsonModels;
using Crytex.Service.IService;
using Microsoft.Practices.Unity;

namespace Crytex.Web.Areas.User
{
    public class UserVmController : UserCrytexController
    {
        private readonly IUserVmService _userVmService;

        public UserVmController([Dependency("Secured")]IUserVmService userVmService)
        {
            this._userVmService = userVmService;
        }

        /// <summary>
        /// Получение списка машин пользователя
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<UserVmViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            return this.GetPageInner(pageNumber, pageSize, userId);
        }

        /// <summary>
        /// Получение машины пользователя по id
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
                BadRequest(ModelState);
            }
            var vm = this._userVmService.GetVmById(guid);
            var model = AutoMapper.Mapper.Map<UserVmViewModel>(vm);

            return Ok(model);
        }
        private IHttpActionResult GetPageInner(int pageNumber, int pageSize, string userId)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }
            var page = this._userVmService.GetPage(pageNumber, pageSize, userId);
            var viewModel = AutoMapper.Mapper.Map<PageModel<UserVmViewModel>>(page);
            return Ok(viewModel);
        }
    }
}
