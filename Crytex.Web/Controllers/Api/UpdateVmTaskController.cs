﻿using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;
using Crytex.Model.Exceptions;

namespace Crytex.Web.Controllers.Api
{
    public class UpdateVmTaskController : CrytexApiController
    {
        private ITaskVmService _taskService;
        public UpdateVmTaskController(ITaskVmService taskService)
        {
            this._taskService = taskService;
        }

        /// <summary>
        /// Метод создания задачи обновления виртуальной машины
        /// </summary>
        /// <param name="model">Параметры создания задачи</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]        
        public IHttpActionResult Post(UpdateVmTaskViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var task = AutoMapper.Mapper.Map<UpdateVmTask>(model);
                var userId = this.CrytexContext.UserInfoProvider.GetUserId();
                UpdateVmTask createdTask;
                if(this.CrytexContext.UserInfoProvider.IsCurrentUserInRole("Admin") ||
                    this.CrytexContext.UserInfoProvider.IsCurrentUserInRole("Support"))
                {
                    createdTask = this._taskService.CreateUpdateVmTask(task);
                }
                else
                {
                    createdTask = this._taskService.CreateUpdateVmTask(task, userId);
                }
                return Ok(new { id = createdTask.Id });
            }

            return BadRequest(this.ModelState);
        }

        /// <summary>
        /// Получение задачи обновления по Id
        /// </summary>
        /// <param name="id">Id задачи</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            UpdateVmTask task;
            if (this.CrytexContext.UserInfoProvider.IsCurrentUserInRole("Admin") ||
                this.CrytexContext.UserInfoProvider.IsCurrentUserInRole("Support"))
            {
                task = this._taskService.GetUpdateTaskById(id);
            }
            else
            {
                task = this._taskService.GetUpdateTaskById(id, userId);
            }
            var model = AutoMapper.Mapper.Map<UpdateVmTaskViewModel>(task);
            return Ok(model);
        }

        [Authorize]
        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize, string userId = null)
        {
            if (userId == null)
            {
                userId = this.CrytexContext.UserInfoProvider.GetUserId();
            }
            else if (!(this.CrytexContext.UserInfoProvider.IsCurrentUserInRole("Admin") ||
                this.CrytexContext.UserInfoProvider.IsCurrentUserInRole("Support")))
            {
                throw new SecurityException("Only Admin and Support user can acces other user UpdateVmTask info");
            }
            if (pageNumber <= 0 || pageSize <= 0)
            {
                this.ModelState.AddModelError("", "PageNumber and PageSize must be grater than 1");
                return BadRequest(this.ModelState);
            }

            var page = this._taskService.GetUpdateVmTasksForUser(pageNumber, pageSize, userId);
            var viewModel = AutoMapper.Mapper.Map<PageModel<UpdateVmTaskViewModel>>(page);
            
            return Ok(viewModel);
        }
    }
}
