using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Service.Model;

namespace Crytex.Service.IService
{
    public interface IOAuthService
    {
        OAuthClientApplication FindClient(String clientId);
        OAuthRefreshToken FindRefreshToken(String refreshTokenId);
        Boolean AddRefreshToken(OAuthRefreshToken token);
        Boolean RemoveRefreshToken(String refreshTokenId);
        Boolean RemoveRefreshToken(OAuthRefreshToken refreshToken);
        Boolean RemoveRefreshToken(RemoveRefreshTokenParams model);
        Boolean RemoveAllExpiredRefreshTokens();
    }
}
