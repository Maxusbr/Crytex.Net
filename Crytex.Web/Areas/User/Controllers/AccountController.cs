using Crytex.Model.Enums;
using Crytex.Model.Models;
using Crytex.Notification;
using Crytex.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace Crytex.Web.Areas.User.Controllers
{
    public class AccountController : UserCrytexController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private NotificationManager _notificationManager { get; set; }

        [HttpPost]
        public IHttpActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, RegisterDate = DateTime.Now };
                var result = _userManager.CreateAsync(user, model.Password).Result;
                if (result.Succeeded)
                {
                    this.SendConfirmationEmailForUser(user);

                    return this.Ok();
                }

                AddErrors(result);
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return this.Conflict();
        }

        [HttpPost]
        public IHttpActionResult SendEmailAgain(string userId)
        {
            if (userId != null)
            {
                var user = _userManager.FindById(userId);

                this.SendConfirmationEmailForUser(user);

                return this.Ok();
            }

            return this.Conflict();
        }

        [HttpPost]
        public IHttpActionResult ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.Conflict();
            }
            var provider = new DpapiDataProtectionProvider("TestWebAPI");
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
            var result = _userManager.ConfirmEmailAsync(userId, code).Result;
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
        public IHttpActionResult UpdateUserInfo(string userId, FullUserInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindById(userId);

                if (user != null)
                {
                    user.Payer = model.Payer;
                    user.UserType = model.UserType;
                    user.ContactPerson = model.ContactPerson;
                    user.PhoneNumber = model.TelephoneNumber;
                    user.City = model.City;
                    user.Country = model.Country;
                }

                var result = _userManager.UpdateAsync(user).Result;

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

        private void SendConfirmationEmailForUser(ApplicationUser user)
        {
            _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false).Wait();

            var provider = new DpapiDataProtectionProvider("TestWebAPI");
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));

            var code = _userManager.GenerateEmailConfirmationTokenAsync(user.Id).Result;

            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
            var mailParams = new List<KeyValuePair<string, string>>();
            mailParams.Add(new KeyValuePair<string, string>("callbackUrl", callbackUrl.ToString()));

            _notificationManager.SendEmailImmediately("crytex@crytex.com", user.Email, EmailTemplateType.Registration, null,
                mailParams, DateTime.Now).Wait();
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