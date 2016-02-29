using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models.GameServers;
using Crytex.Service.Extension;
using Crytex.Service.IService;

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