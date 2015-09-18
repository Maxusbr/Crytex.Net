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

        // GET: api/CreateVmTask/5
        public  IHttpActionResult Get(int id)
        {
            var task = this._taskVmService.GetCreateVmTaskById(id);
            var model = AutoMapper.Mapper.Map<CreateVmTaskAdminViewModel>(task);

            return Ok(model);
        }

        // GET: api/CreateVmTaskAdmin
        public IHttpActionResult Get(int pageNumber, int pageSize, string userId = null, DateTime? from = null, DateTime? to = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            var page = this._taskVmService.GetCreateVmTasksForUser(pageNumber, pageSize, userId, from, to);
            var viewModel = AutoMapper.Mapper.Map<PageModel<CreateVmTaskAdminViewModel>>(page);
            return Ok(viewModel);

        }

        // POST: api/CreateVmTaskAdmin
        public IHttpActionResult Post([FromBody]CreateVmTaskAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTask = AutoMapper.Mapper.Map<CreateVmTask>(model);
            newTask = this._taskVmService.CreateVm(newTask);
            return Ok(new { id = newTask.Id });
        }

        // DELETE: api/CreateVmTaskAdmin/5
        public IHttpActionResult Delete(int id)
        {
            this._taskVmService.DeleteCreateVmTaskById(id);
            return Ok(id);
        }
    }
}
