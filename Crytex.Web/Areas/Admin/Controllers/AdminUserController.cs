using System.Collections.Generic;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Web.Http.Description;
using Crytex.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.BuilderProperties;

namespace Crytex.Web.Areas.Admin
{
    public class AdminUserController : AdminCrytexController
    {
        public AdminUserController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        IApplicationUserService _applicationUserService { get; }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        // GET api/<controller>
        [ResponseType(typeof(PageModel<ApplicationUserViewModel>))]
        public IHttpActionResult Get(int pageSize = 20, int pageIndex = 1, string userName = null, string email = null)
        {
            if (pageIndex <= 0 || pageSize <= 0)
                return BadRequest("PageSize and PageIndex must be positive.");

            var users = _applicationUserService.GetPage(pageSize, pageIndex, userName, email);
            var model = AutoMapper.Mapper.Map<PageModel<ApplicationUserViewModel>>(users);
            return Ok(model);
        }

        /// <summary>
        /// Получение пользователя по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
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
                Areas = model.Areas,
                Address = model.Address,
                CodePhrase = model.CodePhrase,
                UserType = model.UserType
            };

            var creationResult = this.UserManager.CreateAsync(newUser, model.Password).Result;
            if (!creationResult.Succeeded)
            {
                AddErrors(creationResult);
                return BadRequest(this.ModelState);
            }

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
                user.Areas = model.Areas;
                user.Address = model.Address;
                user.CodePhrase = model.CodePhrase;
                user.UserType = model.UserType;

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
            var user = this.UserManager.FindById(id);
            if(user == null)
            {
                return NotFound();
            }
            
            _applicationUserService.DeleteUser(user);

            return Ok();
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