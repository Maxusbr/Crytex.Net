using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Web.Models.JsonModels;
using Project.Model.Models;

namespace Project.Web.Controllers.Api
{
    public class CreateVmTaskController : CrytexApiController
    {
        private readonly ITaskVmService _taskVmService;

        public CreateVmTaskController(ITaskVmService taskVmService)
        {
            this._taskVmService = taskVmService;
        }

        // GET: api/CreateVmTask/5
        [HttpGet]
        public CreateVmTaskViewModel GetById(int id)
        {
            var task = this._taskVmService.GetCreateVmTaskById(id);
            var model = AutoMapper.Mapper.Map<CreateVmTaskViewModel>(task);

            return model;
        }

        // GET: api/CreateVmTask
        [HttpGet]
        public HttpResponseMessage GetPage(int pageNumber, int pageSize, DateTime? from = null, DateTime? to =null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                this.ModelState.AddModelError("", "PageNumber and PageSize must be grater than 1");
            }
            else
            {
                var userId = this.CrytexContext.UserInfoProvider.GetUserId();
                var page = this._taskVmService.GetCreateVmTasksForUser(pageNumber, pageSize, userId, from, to);
                var viewModel = AutoMapper.Mapper.Map<PageModel<CreateVmTaskViewModel>>(page);
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // POST: api/CreateVmTask
        public HttpResponseMessage Post(CreateVmTaskViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var newTask = AutoMapper.Mapper.Map<CreateVmTask>(model);
                newTask.UserId = this.CrytexContext.UserInfoProvider.GetUserId();
                newTask = this._taskVmService.CreateVm(newTask);

                return Request.CreateResponse(HttpStatusCode.Created, new { id = newTask.Id });
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }
    }
}
