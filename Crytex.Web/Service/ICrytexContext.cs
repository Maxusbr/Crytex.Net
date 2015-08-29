namespace Crytex.Web.Service
{
    public interface ICrytexContext
    {
        IServerConfig ServerConfig { get; }
        IHttp Http { get; }
        IUserInfoProvider UserInfoProvider { get; }
    }
}
