using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Service.Model;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Service.Extension;
using Crytex.Model.Helpers;

namespace Crytex.Service.Service
{
    public class OAuthService : IOAuthService
    {
        private readonly IOAuthClientApplicationRepository  _oauthClientApplicationRepository;
        private readonly IOAuthRefreshTokenRepository       _oauthRefreshTokenRepository;
        private readonly IUnitOfWork                        _unitOfWork;

        public OAuthService(IUnitOfWork unitOfWork, IOAuthClientApplicationRepository oauthClientApplicationRepository, IOAuthRefreshTokenRepository oauthRefreshTokenRepository)
        {
            this._unitOfWork = unitOfWork;
            this._oauthClientApplicationRepository = oauthClientApplicationRepository;
            this._oauthRefreshTokenRepository = oauthRefreshTokenRepository;
        }

        public Boolean AddRefreshToken(OAuthRefreshToken token)
        {
            var result = true;

            try
            {
                var existingToken = this._oauthRefreshTokenRepository.Get(r => r.Id == token.Id);

                if (existingToken != null)
                {
                    RemoveRefreshToken(existingToken);
                }

                _oauthRefreshTokenRepository.Add(token);

                this._unitOfWork.Commit();
            }
            catch
            {
                this._unitOfWork.Rollback();
                result = false;
            }

            return result;
        }

        public OAuthClientApplication FindClient(String clientId)
        {
            var client = this._oauthClientApplicationRepository.GetById(clientId);

            return client;
        }

        public OAuthRefreshToken FindRefreshToken(String refreshTokenId)
        {
            var refreshToken = this._oauthRefreshTokenRepository.GetById(refreshTokenId);

            return refreshToken;
        }

        public Boolean RemoveAllExpiredRefreshTokens()
        {
            var result = true;

            var removeDate = DateTime.UtcNow.AddDays(1);

            var tokensToRemove = this._oauthRefreshTokenRepository.GetMany(rt => removeDate > rt.ExpiresUtc);

            if (tokensToRemove.Any())
            {
                try
                {
                    this._oauthRefreshTokenRepository.Delete(r => tokensToRemove.Contains(r));
                    this._unitOfWork.Commit();
                }
                catch
                {
                    this._unitOfWork.Rollback();
                    result = false;
                }
            }
            return result;
        }

        public Boolean RemoveRefreshToken(RemoveRefreshTokenParams model)
        {
            var hashedId = Helper.GetHash(model.RefreshToken);
            var tokenToRemove = this._oauthRefreshTokenRepository.Get(t => t.Subject == model.UserName && t.Id == hashedId);

            if (tokenToRemove == null) return false;

            var result = true;

            try
            {
                this._oauthRefreshTokenRepository.Delete(tokenToRemove);
                this._unitOfWork.Commit();
            }
            catch
            {
                this._unitOfWork.Rollback();
                result = false;
            }

            return result;
        }

        public Boolean RemoveRefreshToken(OAuthRefreshToken refreshToken)
        {
            var result = true;
            try
            {
                this._oauthRefreshTokenRepository.Delete(refreshToken);
                this._unitOfWork.Commit();
            }
            catch
            {
                this._unitOfWork.Rollback();
                result = false;
            }
            return result;
        }

        public Boolean RemoveRefreshToken(String refreshTokenId)
        {
            var result = true;

            var refreshToken = this._oauthRefreshTokenRepository.GetById(refreshTokenId);
            if (refreshToken != null)
            {
                try
                {
                    this._oauthRefreshTokenRepository.Delete(refreshToken);
                    this._unitOfWork.Commit();
                }
                catch
                {
                    this._unitOfWork.Rollback();
                    result = false;
                }                
            }

            return result;
        }
    }
}
