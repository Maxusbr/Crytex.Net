using System;
using Crytex.Model.Models.GameServers;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IGameHostService
    {
        GameHost Create(GameHostCreateOptions options);
        GameHost GetGameHostWithAvalailableSlot(int gameId);
        void Update(Int32 id, GameHostCreateOptions option);
        void Delete(Int32 id);
        IPagedList<GameHost> GetPage(int pageNumber, int pageSize);
    }
}