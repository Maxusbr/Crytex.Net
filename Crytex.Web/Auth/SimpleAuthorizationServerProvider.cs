using Crytex.Model.Helpers;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Service;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Crytex.Web.Auth
{
    /// <summary>
    /// Provides the work with access token
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        #region private injected sources

        private ApplicationUserManager _applicationUserManager { get; set; }
        private IUserLoginLogService _loginLogService { get; set; }

        private OAuthService _oauthService { get; set; }

        #endregion

        public SimpleAuthorizationServerProvider()
        {
            var unityContainer = Crytex.Web.App_Start.UnityConfig.GetConfiguredContainer();
            this._applicationUserManager = (ApplicationUserManager)unityContainer.Resolve(typeof(ApplicationUserManager), "");
            this._oauthService = (OAuthService)unityContainer.Resolve(typeof(OAuthService), "");
            this._loginLogService = (IUserLoginLogService)unityContainer.Resolve(typeof(IUserLoginLogService), "");
        }

        /// <summary>
        /// This method is responsible for validating the "Client"
        /// </summary>
        /// <param name="context">Current authentication context</param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            context.OwinContext.Response.Headers.Set("Access-Control-Allow-Origin", allowedOrigin);

            var clientId = string.Empty;
            var clientSecret = string.Empty;
            OAuthClientApplication client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.Validated();//context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            client = _oauthService.FindClient(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.EnumApplicationType == Model.Enums.EnumApplicationType.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// This method is responsible to validate the username and password sent 
        /// to the authorization server’s token endpoint
        /// </summary>
        /// <param name="context">Current authentication context</param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            ApplicationUser user = await _applicationUserManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            if (user.IsBlocked)
            {
                context.SetError("user_blocked", "This user account is blocked", user.Id);
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "client_id", context.ClientId ?? string.Empty
                }
                ,{
                    "userName", context.UserName
                }
                ,{
                    "userId", user.Id
                }
                ,{
                    "refreshTokenId", ""
                }
                //,{
                //    "initials", user.FirstName + " " user.LastName
                //}
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

            this._loginLogService.CreateLogEntryForNow(user.Id, context.Request.RemoteIpAddress, context.ClientId != null);
        }

        /// <summary>
        /// This method implements the logic which allows us to issue new claims or updating existing claims 
        /// and contain them into the new access token generated before sending it to the user
        /// </summary>
        /// <param name="context">Current authentication context</param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            var newClaim = newIdentity.Claims.FirstOrDefault(c => c.Type == "newClaim");
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            //need do this cause user may change first or last name
            //var userId = context.Ticket.Properties.Dictionary["userId"];
            //ApplicationUser user = await applicationUserManager.FindByIdAsync(userId);
            //context.Ticket.Properties.Dictionary["initials"] = user.FirstName + " " + user.LastName;

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}