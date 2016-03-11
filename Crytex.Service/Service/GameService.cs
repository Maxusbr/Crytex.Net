using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Enums;
using Crytex.Model.Exceptions;
using Crytex.Model.Models.GameServers;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using PagedList;

namespace Crytex.Service.Service
{
    class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GameService(IGameRepository gameRepository, IUnitOfWork unitOfWork)
        {
            this._gameRepository = gameRepository;
            this._unitOfWork = unitOfWork;
        }

        public Game Create(Game game)
        {
            this.ValidateGame(game);

            _gameRepository.Add(game);
            _unitOfWork.Commit();

            return game;
        }

        public IEnumerable<Game> GetGamesByIds(IEnumerable<int> gameIds)
        {
            if (!gameIds.Any())
            {
                throw new ApplicationException("Ids collection is empty");
            }

            Expression<Func<Game, bool>> where = x => false;
            foreach (var id in gameIds)
            {
                where = where.Or(x => x.Id == id);
            }


            var games = _gameRepository.GetMany(where);

            return games;
        }

        public IPagedList<Game> GetPage(int pageNumber, int pageSize, GameFamily family)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<Game, bool>> where = x => true;
            if (family != GameFamily.Unknown)
            {
                where = where.And(p => p.Family == family);
            }
            var pagedList = _gameRepository.GetPage(pageInfo, where, x => x.Id, false, x => x.GameHosts);

            return pagedList;
        }

        public void Update(Game game)
        {
            var existGame = _gameRepository.GetById(game.Id);
            if (existGame == null)
            {
                throw new ValidationException($"Game with Id={game.Id} not found");
            }
            if(!string.IsNullOrEmpty(game.Name))
                existGame.Name = game.Name;
            if(!string.IsNullOrEmpty(game.Version))
                existGame.Version = game.Version;
            if (!string.IsNullOrEmpty(game.VersionCode))
                existGame.VersionCode = game.VersionCode;
            existGame.Family = game.Family;

            _gameRepository.Update(existGame);
            _unitOfWork.Commit();
        }

        public void Delete(Int32 Id)
        {
            var existGame = _gameRepository.GetById(Id);
            if (existGame == null)
            {
                throw new ValidationException($"Game with Id={Id} not found");
            }

            _gameRepository.Delete(existGame);
            _unitOfWork.Commit();
        }

        private void ValidateGame(Game game)
        {
            if (game.Name == null)
            {
                throw new ValidationException("Game name cannot be NULL");
            }
            if (game.Version == null)
            {
                throw new ValidationException("Game version cannot be NULL");
            }
        }
    }
}