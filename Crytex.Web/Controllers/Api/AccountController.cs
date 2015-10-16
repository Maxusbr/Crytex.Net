using Crytex.Model.Models;
using Crytex.Service.Model;
using Crytex.Service.Service;
using Crytex.Web.Models.JsonModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace Crytex.Web.Controllers.Api
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager _userManager;
        private OAuthService _oauthService;
        
        public AccountController(ApplicationUserManager userManager, OAuthService oauthService)
        {
            _userManager = userManager;
            _oauthService = oauthService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(SignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok("User was successfully signed up.");
        }

        [HttpPost]
        [Authorize]
        [Route("removeRefreshToken")]
        public IHttpActionResult RemoveRefreshToken(RemoveRefreshTokenParams model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _oauthService.RemoveRefreshToken(model);

            return Ok("Refresh token was successfuly removed.");
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
