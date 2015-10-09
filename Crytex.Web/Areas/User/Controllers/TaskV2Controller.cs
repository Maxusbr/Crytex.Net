using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;

namespace Crytex.Web.Areas.User
{
    public class TaskV2Controller : UserCrytexController
    {
        private readonly ITaskV2Service _taskService;

        public TaskV2Controller(ITaskV2Service taskService)
        {
            this._taskService = taskService;
        }

        /// <summary>
        /// Получение списка TaskV2
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        // GET: api/TaskV2
        [ResponseType(typeof(PageModel<TaskV2ViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri]TaskV2SearchParamsViewModel searchParams = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IPagedList<TaskV2> tasks = new PagedList<TaskV2>(new List<TaskV2>(), pageNumber, pageSize);

            if (searchParams != null)
            {
                var taskV2Params = AutoMapper.Mapper.Map<TaskV2SearchParams>(searchParams);
                tasks = _taskService.GetPageTasks(pageNumber, pageSize, taskV2Params);
            }
            else
            {
                tasks = _taskService.GetPageTasks(pageNumber, pageSize);
            }

            var viewTasks = AutoMapper.Mapper.Map<PageModel<TaskV2ViewModel>>(tasks);
            return Ok(viewTasks);
        }

        /// <summary>
        /// Получение TaskV2 по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/TaskV2/5
        [ResponseType(typeof(TaskV2ViewModel))]
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                return BadRequest("Invalid Guid format");
            var task = _taskService.GetTaskById(guid);
            var viewTask = AutoMapper.Mapper.Map<TaskV2ViewModel>(task);
            return Ok(viewTask);
        }

        /// <summary>
        /// Создание нового TaskV2
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        // POST: api/TaskV2
        public IHttpActionResult Post([FromBody]TaskV2ViewModel task)
        {
            if (!ModelState.IsValid || task == null)
                return BadRequest(ModelState);

            if (task.TypeTask == TypeTask.UpdateVm || task.TypeTask == TypeTask.CreateVm)
            {
                if (!IsValidOptions<ConfigVmOptions>(task.Options)) {
                    ModelState.AddModelError("Options", "Not Valid Options for this type Task");
                    return BadRequest(ModelState);
                }
            }
            else if (task.TypeTask == TypeTask.ChangeStatus)
            {
                if (!IsValidOptions<ChangeStatusOptions>(task.Options))
                {
                    ModelState.AddModelError("Options", "Not Valid Options for this type Task");
                    return BadRequest(ModelState);
                }
            }
            var modelTask = AutoMapper.Mapper.Map<TaskV2>(task);

            var newTask = _taskService.CreateTask(modelTask, modelTask.Options);

            return Ok(newTask);
        }

        /// <summary>
        /// Обновление TaskV2
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT: api/TaskV2/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Ok();
        }

        /// <summary>
        /// Уланение TaskV2 по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/TaskV2/5
        public IHttpActionResult Delete(Guid id)
        {
            var task = _taskService.GetTaskById(id);
            if(task.StatusTask != StatusTask.End)
                return BadRequest("Status task will be End");
            _taskService.RemoveTask(task.Id);
            return Ok();
        }

        private bool IsValidOptions<T>(string strOptions) where T : BaseOptions
        {
            try
            {
                JsonConvert.DeserializeObject<T>(strOptions);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
