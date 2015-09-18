using System.Collections.Generic;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using Microsoft.AspNet.Identity;

namespace Crytex.Web.Controllers.Api.Admin
{
    public class UserController : CrytexApiController
    {
        public UserController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        IApplicationUserService _applicationUserService { get; }

        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id is null or empty.");

            var user = _applicationUserService.GetUserById(id);
            var model = AutoMapper.Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
            return Ok(model);
        }

        // GET api/<controller>
        public IHttpActionResult Get(int pageSize = 20, int pageIndex = 1, string userName = null, string email = null)
        {
            if (pageIndex <= 0 || pageSize <= 0)
                return BadRequest("PageSize and PageIndex must be positive.");

            var  users = _applicationUserService.GetPage(pageSize, pageIndex, userName,email);
            var model = AutoMapper.Mapper.Map<List<ApplicationUser>, List<ApplicationUserViewModel>>(users);
            return Ok(model);
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]ApplicationUserViewModel model)
        {
            if (model.ValidateForCreationScenario() && this.ModelState.IsValid)
            {
                return BadRequest("Some params are empty. UserName, Password and Email are required");
            }

            var newUser = new ApplicationUser { UserName = model.UserName, Email = model.Email };

            var creationResult = this.UserManager.CreateAsync(newUser, model.Password).Result;
            if (!creationResult.Succeeded)
            {
                AddErrors(creationResult);
                return BadRequest(this.ModelState);
            }

            return Created(Url.Link("DefaultApi", new { controller = "User", id = newUser.Id }), new { id = newUser.Id });
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(string id,ApplicationUserViewModel model)
        {
            if (!(model.ValidateForEditingScenario() && this.ModelState.IsValid))
            {
                return BadRequest("Some params are empty. UserName or Password or Email are required");
            }
            // If password is set - update password using UserManager
            if (!string.IsNullOrEmpty(model.Password))
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
            if (!(string.IsNullOrEmpty(model.UserName) && string.IsNullOrEmpty(model.Email)))
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
            this.UserManager.Delete(user);

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