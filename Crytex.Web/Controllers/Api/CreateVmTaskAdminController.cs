using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Crytex.Web.Controllers.Api
{
    public class CreateVmTaskAdminController : ApiController
    {
        private readonly ITaskVmService _taskVmService;

        public CreateVmTaskAdminController(ITaskVmService taskVmService)
        {
            this._taskVmService = taskVmService;
        }

        // GET: api/CreateVmTaskAdmin
        /// <summary>
        /// Получение страницы задач создания
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPage(int pageNumber, int pageSize, string userId = null, DateTime? from = null, DateTime? to = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                this.ModelState.AddModelError("", "PageNumber and PageSize must be grater than 1");
            }
            else
            {
                var page = this._taskVmService.GetCreateVmTasksForUser(pageNumber, pageSize, userId, from, to);
                var viewModel = AutoMapper.Mapper.Map<PageModel<CreateVmTaskAdminViewModel>>(page);
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // GET: api/CreateVmTask/5
        [HttpGet]
        public CreateVmTaskAdminViewModel GetById(int id)
        {
            var task = this._taskVmService.GetCreateVmTaskById(id);
            var model = AutoMapper.Mapper.Map<CreateVmTaskAdminViewModel>(task);

            return model;
        }

        // POST: api/CreateVmTaskAdmin
        public HttpResponseMessage Create(CreateVmTaskAdminViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var newTask = AutoMapper.Mapper.Map<CreateVmTask>(model);
                newTask = this._taskVmService.CreateVm(newTask);

                return Request.CreateResponse(HttpStatusCode.Created, new { id = newTask.Id });
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        // DELETE: api/CreateVmTaskAdmin/5
        public HttpResponseMessage Delete(int id)
        {
            this._taskVmService.DeleteCreateVmTaskById(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
