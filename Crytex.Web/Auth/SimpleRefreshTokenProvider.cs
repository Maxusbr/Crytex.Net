using Crytex.Model.Helpers;
using Crytex.Model.Models;
using Crytex.Service.Service;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Web.Auth
{
    /// <summary>
    /// Provides the work with refresh token
    /// </summary>
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {

        #region private injected sources

        private OAuthService _oauthService { get; set; }

        #endregion
        public SimpleRefreshTokenProvider()
        {
            var unityContainer = Crytex.Web.App_Start.UnityConfig.GetConfiguredContainer();
            this._oauthService = (OAuthService)unityContainer.Resolve(typeof(OAuthService), "");
        }

        /// <summary>
        /// This method is responsible to create or recreate refresh token
        /// </summary>
        /// <param name="context">Current authentication cotext</param>
        /// <returns></returns>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientId = context.Ticket.Properties.Dictionary["client_id"];

            if (String.IsNullOrEmpty(clientId))
            {
                return;
            }

            var refreshTokenId = context.Ticket.Properties.Dictionary["refreshTokenId"];
            if (string.IsNullOrWhiteSpace(refreshTokenId))
            {
                refreshTokenId = Guid.NewGuid().ToString("n");
                context.Ticket.Properties.Dictionary["refreshTokenId"] = refreshTokenId;
            }

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new OAuthRefreshToken()
            {
                Id = Helper.GetHash(refreshTokenId),
                ClientId = clientId,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result =  _oauthService.AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }

            await Task.FromResult<object>(null);
        }

        /// <summary>
        /// This method implements the logic needed once we receive the refresh token
        /// so we can generate a new access token 
        /// </summary>
        /// <param name="context">Current authentication cotext</param>
        /// <returns></returns>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string hashedTokenId = Helper.GetHash(context.Token);

            var refreshToken = _oauthService.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = _oauthService.RemoveRefreshToken(hashedTokenId);
            }

            await Task.FromResult<object>(null);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}
