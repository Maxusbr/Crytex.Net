using Crytex.Core.Service;

namespace Crytex.Web.Service
{
    public class CrytexContext : ICrytexContext
    {
        public CrytexContext(IServerConfig serverConfig, IHttp http, IUserInfoProvider userInfoProvider)
        {
            this.ServerConfig = serverConfig;
            this.Http = http;
            this.UserInfoProvider = userInfoProvider;
        }

        public IHttp Http { get; private set; }

        public IServerConfig ServerConfig { get; private set; }

        public IUserInfoProvider UserInfoProvider { get; private set; }
    }
}