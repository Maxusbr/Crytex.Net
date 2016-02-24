using Crytex.Model.Models.GameServers;
using Crytex.Service.Model;

namespace Crytex.Service.IService
{
    public interface IGameHostService
    {
        GameHost Create(GameHostCreateOptions options);
    }
}