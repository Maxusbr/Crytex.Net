using Crytex.Model.Enums;
using Crytex.Model.Models;
using Crytex.Notification;
using Crytex.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Routing;
using Crytex.Service.Model;
using Crytex.Service.Service;
using Crytex.Web.Controllers.Api;
using Crytex.Web.Helpers;

namespace Crytex.Web.Areas.User.Controllers
{
    public class AccountController : CrytexApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private OAuthService _oauthService;
        private INotificationManager _notificationManager { get; set; }

        public AccountController(ApplicationUserManager userManager, OAuthService oauthService, INotificationManager notificationManager, ApplicationSignInManager applicationSignInManager)
        {
            _userManager = userManager;
            _oauthService = oauthService;
            _signInManager = applicationSignInManager;
            _notificationManager = notificationManager;
        }


        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.email, Email = model.email, RegisterDate = DateTime.Now };
                var result = await _userManager.CreateAsync(user, model.password);

                if (result.Succeeded)
                {

                    await this.SendConfirmationEmailForUser(user);


                    var addRoleResult = await _userManager.AddToRoleAsync(user.Id, "FirstStepRegister");
                    if (addRoleResult.Succeeded)
                    {
                        return this.Ok();
                    }
                    else
                    {
                        return InternalServerError();
                    }

                }
                return new CrytexResult(ServerTypesResult.UserExist);
            }
            return this.Conflict();
        }

        [HttpPost]
        public async Task<IHttpActionResult> SendEmailAgain(string userId)
        {
            if (userId != null)
            {
                var user = _userManager.FindById(userId);

                await this.SendConfirmationEmailForUser(user);

                return this.Ok();
            }

            return this.Conflict();
        }

        [HttpPost]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmEmailModel confirmEmail)
        {
            if (confirmEmail.userId == null || confirmEmail.code == null)
            {
                return this.Conflict();
            }

            var code = Base64ForUrlDecode(confirmEmail.code);
            var result = await _userManager.ConfirmEmailAsync(confirmEmail.userId, code);
            if (result.Succeeded)
            {
                return this.Ok();
            }
            else
            {
                return this.Conflict();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUserInfo(FullUserInfoViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(CrytexContext.UserInfoProvider.GetUserId());
                if (!user.PhoneNumberConfirmed)
                {
                    return this.Conflict();
                }
                user.Name = model.Name;
                user.Lastname = model.LastName;
                user.CodePhrase = model.CodePhrase;
                user.Patronymic = model.Patronymic;
                user.Region = model.Region;
                user.Address = model.Address;
                user.UserType = model.UserType;
                user.City = model.City;
                user.Country = model.Country;

                var result = await _userManager.UpdateAsync(user);
                var addRoleResult = await _userManager.RemoveFromRoleAsync(user.Id, "FirstStepRegister");
                if (!addRoleResult.Succeeded)
                {
                    return this.Conflict();
                }
                addRoleResult = await _userManager.AddToRoleAsync(user.Id, "User");
                if (addRoleResult.Succeeded)
                {
                    return this.Ok();
                }

                if (!result.Succeeded)
                {
                    AddErrors(result);
                }
                return this.Ok();
            }

            return this.Conflict();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IHttpActionResult> EditUser(FullUserInfoViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(CrytexContext.UserInfoProvider.GetUserId());

                user.Name = model.Name;
                user.Lastname = model.LastName;
                user.CodePhrase = model.CodePhrase;
                user.Patronymic = model.Patronymic;
                user.Region = model.Region;
                user.Address = model.Address;
                user.UserType = model.UserType;
                user.City = model.City;
                user.Country = model.Country;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    AddErrors(result);
                }
                return this.Ok();
            }

            return this.Conflict();
        }
        [HttpPost]
        public IHttpActionResult AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Generate the token 
                var code = UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number).Result;
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                // Send token
                UserManager.SmsService.SendAsync(message).Wait();

                return this.Ok();
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpPost]
        public IHttpActionResult VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {

            if (this.ModelState.IsValid)
            {
                var result = UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code).Result;
                if (result.Succeeded)
                {
                    return this.Ok();
                }
                ModelState.AddModelError("", "Failed to verify phone");
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult RemoveRefreshToken(RemoveRefreshTokenParams model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _oauthService.RemoveRefreshToken(model);

            return Ok("Refresh token was successfuly removed.");
        }

        private async Task SendConfirmationEmailForUser(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = $"{CrytexContext.ServerConfig.GetClientAddress()}#/account/verify?userId={user.Id}&&code={Base64ForUrlEncode(code)}";
            var mailParams = new List<KeyValuePair<string, string>>();
            mailParams.Add(new KeyValuePair<string, string>("callbackUrl", callbackUrl));

            await _notificationManager.SendEmailImmediately("crytex@crytex.com", user.Email, EmailTemplateType.Registration, null,
               mailParams, DateTime.Now);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (model.email != null)
            {
                var user = await _userManager.FindByEmailAsync(model.email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User with this Email not found");
                    return BadRequest(ModelState);
                }
                await SendResetPasswordEmailForUser(user);

                return this.Ok();
            }

            return this.Conflict();
        }

        private async Task SendResetPasswordEmailForUser(ApplicationUser user)
        {

            var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

            var callbackUrl = $"{CrytexContext.ServerConfig.GetClientAddress()}#/account/resetPassword?userId={user.Id}&&code={Base64ForUrlEncode(code)}";
            var mailParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("callbackUrl", callbackUrl)
            };

            await _notificationManager.SendEmailImmediately("crytex@crytex.com", user.Email, EmailTemplateType.ResetPassword, null,
               mailParams, DateTime.Now);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateNewPassword(CreateNewPasswordModel model)
        {
            if (model.userId == null || model.code == null || model.password == null)
            {
                return this.Conflict();
            }

            var code = Base64ForUrlDecode(model.code);
            var result = await _userManager.ResetPasswordAsync(model.userId, code, model.password);
            if (result.Succeeded)
            {
                return this.Ok();
            }
            else
            {
                return this.Conflict();
            }
        }

        public class CreateNewPasswordModel
        {
            public string userId { get; set; }
            public string code { get; set; }
            public string password { get; set; }
        }

        public class ResetPasswordModel
        {
            public string email { get; set; }
        }

        public class ConfirmEmailModel
        {
            public string userId { get; set; }
            public string code { get; set; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public static string Base64ForUrlEncode(string str)
        {
            var encbuff = Encoding.UTF8.GetBytes(str);
            return HttpServerUtility.UrlTokenEncode(encbuff);
        }

        public static string Base64ForUrlDecode(string str)
        {
            var decbuff = HttpServerUtility.UrlTokenDecode(str);
            return decbuff != null ? Encoding.UTF8.GetString(decbuff) : null;
        }
    }
}