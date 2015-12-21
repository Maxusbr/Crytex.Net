using System;
using System.Collections.Generic;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http.Description;
using Crytex.Service.Model;
using Crytex.Web.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Crytex.Web.Areas.Admin
{
    public class AdminUserController : AdminCrytexController
    {
        public AdminUserController(IApplicationUserService applicationUserService, ITaskV2Service taskService, IBilingService billingService, ITriggerService triggerService)
        {
            _applicationUserService = applicationUserService;
            _taskService = taskService;
			_triggerService = triggerService;
            _billingService = billingService;
        }

        private IApplicationUserService _applicationUserService { get; }
        private ITaskV2Service _taskService { get; set; }
		private  ITriggerService _triggerService { get; set; }
        private IBilingService _billingService { get; set; }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        // GET api/Admin/<controller>
        [ResponseType(typeof(PageModel<ApplicationUserViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri]AdminApplicationUserSearchParamsViewModel searchParams = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageSize and PageNumber must be positive.");

            IPagedList<ApplicationUser> users = new PagedList<ApplicationUser>(new List<ApplicationUser>(), pageNumber, pageSize);

            if (searchParams != null)
            {
                var applicationUsersParams = AutoMapper.Mapper.Map<ApplicationUserSearchParams>(searchParams);
                users = _applicationUserService.GetPage(pageNumber, pageSize, applicationUsersParams);
            }
            else
            {
                users = _applicationUserService.GetPage(pageNumber, pageSize);
            }

            var viewUsers = AutoMapper.Mapper.Map<PageModel<ApplicationUserViewModel>>(users);
            return Ok(viewUsers);          
        }

        /// <summary>
        /// Получение пользователя по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/Admin/<controller>/5
        [ResponseType(typeof(ApplicationUserViewModel))]
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id is null or empty.");

            var user = _applicationUserService.GetUserById(id);
            var model = AutoMapper.Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
            return Ok(model);
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]ApplicationUserViewModel model)
        {
            if (!model.ValidateForCreationScenario() || !this.ModelState.IsValid)
            {
                return BadRequest("Some params are empty. UserName, Password and Email are required");
            }

            var newUser = new ApplicationUser {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                Patronymic = model.Patronymic,
                City = model.City,
                Address = model.Address,
                CodePhrase = model.CodePhrase,
                UserType = model.UserType,
                Country = model.Country,
                ContactPerson = model.ContactPerson,
                Payer = model.Payer,
				RegisterDate = DateTime.UtcNow
            };

            var creationResult = this.UserManager.CreateAsync(newUser, model.Password).Result;
            if (!creationResult.Succeeded)
            {
                AddErrors(creationResult);
                return BadRequest(this.ModelState);
            }

            _triggerService.InitStandartTriggersForUser(newUser.Id);

            return Created(Url.Link("DefaultApiAdmin", new { controller = "AdminUser", id = newUser.Id }), new { id = newUser.Id });
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(string id, [FromBody]ApplicationUserViewModel model)
        {
            if (!model.ValidateForEditingScenario() || !this.ModelState.IsValid)
            {
                return BadRequest("Some params are empty. UserName or Password or Email are required");
            }
            // If password is set - update password using UserManager
            if (!string.IsNullOrEmpty(model.Password) && model.ChangePassword)
            {
                this.UserManager.RemovePassword(id);
                var editResult = this.UserManager.AddPassword(id, model.Password);

                if (!editResult.Succeeded)
                {
                    AddErrors(editResult);
                    return BadRequest(this.ModelState);
                }
            }

            // If UserName or Email are set - update them via UserService
            if (!(string.IsNullOrEmpty(model.UserName) && string.IsNullOrEmpty(model.Email)) && !model.ChangePassword)
            {
                var user = this.UserManager.FindById(id);
                if (!string.IsNullOrEmpty(model.UserName))
                {
                    user.UserName = model.UserName;
                }
                if (!string.IsNullOrEmpty(model.Email))
                {
                    user.Email = model.Email;
                }

                user.Name = model.Name;
                user.Patronymic = model.Patronymic;
                user.City = model.City;
                user.Address = model.Address;
                user.CodePhrase = model.CodePhrase;
                user.UserType = model.UserType;
                user.Country = model.Country;
                user.ContactPerson = model.ContactPerson;
                user.Payer = model.Payer;

                var updateResult = this.UserManager.Update(user);

                if (!updateResult.Succeeded)
                {
                    AddErrors(updateResult);
                    return BadRequest(this.ModelState);
                }
            }

            return Ok();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(string id)
        {
            _applicationUserService.DeleteUser(id);
            _taskService.StopAllUserMachines(id);
            
            return Ok();
        }

        /// <summary>
        /// Обновление баланса пользователя
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("api/AdminUser/UpdateBalance"), HttpPost()]
        public IHttpActionResult UpdateBalance([FromBody]UpdateUserBalance data)
        {
            if (data.Amount == 0)
                return BadRequest("amount can't be 0");

            var billingTransaction = _billingService.UpdateUserBalance(data);

            return Ok(billingTransaction);
        }

        /// <summary>
        /// Управление состоянием пользователя
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("api/AdminUser/UpdateState"), HttpPost()]
        public IHttpActionResult UpdateState([FromBody] UpdateUserState data)
        {
            _applicationUserService.UpdateStateUser(data);

            return Ok();
        }

        /// <summary>
        /// Поиск пользователя по email или userName
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        // GET api/Admin/UserSearch
        [Route("api/Admin/UserSearch")]
        [HttpGet]
        [ResponseType(typeof(List<SimpleApplicationUserViewModel>))]
        public IHttpActionResult Search(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
                return Ok("Username or Email is null or empty.");

            var users = _applicationUserService.Search(searchValue);
            var models = AutoMapper.Mapper.Map<List<ApplicationUser>, List<SimpleApplicationUserViewModel>>(users);
            return Ok(models);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}