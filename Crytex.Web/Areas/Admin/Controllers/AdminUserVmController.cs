using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Web.Models.JsonModels;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Areas.Admin;
using PagedList;


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
        /// <param name="searchParams"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<UserVmViewModel>))]
        [Authorize]
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri]AdminUserVmSearchParamsViewModel searchParams = null)
        {     
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IPagedList<UserVm> machines = new PagedList<UserVm>(new List<UserVm>(), pageNumber, pageSize);

            var machineParams = AutoMapper.Mapper.Map<UserVmSearchParams>(searchParams);
            machines = _userVmService.GetPage(pageNumber, pageSize, machineParams);

            var viewMachines = AutoMapper.Mapper.Map<PageModel<UserVmViewModel>>(machines);
            return Ok(viewMachines);
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
    }
}
