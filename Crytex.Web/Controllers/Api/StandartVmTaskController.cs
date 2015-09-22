using System;
using System.Collections.Generic;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Crytex.Web.Service;
using Microsoft.AspNet.Identity;

namespace Crytex.Web.Controllers.Api
{
    public class StandartVmTaskController : CrytexApiController
    {
        public StandartVmTaskController(IStandartVmTaskService standartVmTaskService)
        {
            _standartVmTaskService = standartVmTaskService;
        }

        IStandartVmTaskService _standartVmTaskService { get; }

        // GET api/<controller>
        public IHttpActionResult Get(int pageSize = 20, int pageNumber = 1, DateTime? dateFrom = null, DateTime? dateTo = null, string userId = null, string vmId = null)
        {
            List<StandartVmTask> tasks = new List<StandartVmTask>();

            var userInfoProvider = CrytexContext.UserInfoProvider;
            List<StandartVmTaskViewModel> model;
            Guid vmGuidId = new Guid(vmId);
            if (userInfoProvider.IsCurrentUserAdmin() || userInfoProvider.IsCurrentUserSupport() || _standartVmTaskService.IsOwnerVm(vmGuidId, userInfoProvider.GetUserId()))
            {
                tasks = _standartVmTaskService.GetPageByVmId(pageSize, pageNumber, dateFrom, dateTo, vmGuidId);
                model = AutoMapper.Mapper.Map<List<StandartVmTask>, List<StandartVmTaskViewModel>>(tasks);
                return Ok(model);
            }

            userId = userId ?? userInfoProvider.GetUserId();
            tasks = _standartVmTaskService.GetPageByUserId(pageSize, pageNumber, dateFrom, dateTo, userId);
            model = AutoMapper.Mapper.Map<List<StandartVmTask>, List<StandartVmTaskViewModel>>(tasks);

            return Ok(model);
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var task = _standartVmTaskService.GetTaskById(id);
            if (task == null)
                return NotFound();

            var model = AutoMapper.Mapper.Map<StandartVmTask, StandartVmTaskViewModel>(task);

            return Ok(model);
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody]CreateStandartVmTaskViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = _standartVmTaskService.Create(new Guid(model.VmId), model.TaskType, model.Virtualization, User.Identity.GetUserId());
            var location = Request.RequestUri + "/" + task.Id;

            return Ok(new { id = task.Id });
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            _standartVmTaskService.Delete(id);
            return Ok();
        }
    }
}