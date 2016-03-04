using Crytex.Model.Models.GameServers;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IGameHostService
    {
        GameHost Create(GameHostCreateOptions options);
        GameHost GetGameHostWithAvalailableSlot(int gameId);
        void Update(int id, GameHostCreateOptions option);
        IPagedList<GameHost> GetPage(int pageNumber, int pageSize);
    }
}