using Crytex.Service.IService;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;

namespace Crytex.Web.Controllers.Api
{
    public class CreateVmTaskController : CrytexApiController
    {
        private readonly ITaskVmService _taskVmService;

        public CreateVmTaskController(ITaskVmService taskVmService)
        {
            this._taskVmService = taskVmService;
        }

        // GET: api/CreateVmTask
        public IHttpActionResult Get(int pageNumber, int pageSize, DateTime? from = null, DateTime? to =null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                BadRequest("PageNumber and PageSize must be grater than 1");
            }
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var page = this._taskVmService.GetCreateVmTasksForUser(pageNumber, pageSize, userId, from, to);
            var viewModel = AutoMapper.Mapper.Map<PageModel<CreateVmTaskViewModel>>(page);

            return Ok(viewModel);
        }

        // POST: api/CreateVmTask
        public IHttpActionResult Post(CreateVmTaskViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var newTask = AutoMapper.Mapper.Map<CreateVmTask>(model);
            newTask.UserId = this.CrytexContext.UserInfoProvider.GetUserId();
            newTask = this._taskVmService.CreateVm(newTask);

            return Ok(new { id = newTask.Id });
        }
    }
}
