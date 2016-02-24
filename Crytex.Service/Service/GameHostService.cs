using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.GameServers;
using Crytex.Service.IService;
using Crytex.Service.Model;

namespace Crytex.Service.Service
{
    public class GameHostService : IGameHostService
    {
        private readonly IGameService _gameSerice;
        private readonly IGameHostRepository _gameHostRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GameHostService(IGameService gameSerice, IGameHostRepository gameHostRepository, IUnitOfWork unitOfWork)
        {
            _gameSerice = gameSerice;
            _gameHostRepository = gameHostRepository;
            _unitOfWork = unitOfWork;
        }


        public GameHost Create(GameHostCreateOptions options)
        {
            var host = new GameHost
            {
                ServerAddress = options.ServerAddress,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password,

            };

            this.ValidateHostEntity(host);

            if (options.SupportedGamesIds != null && options.SupportedGamesIds.Any())
            {
                var hostGames = _gameSerice.GetGamesByIds(options.SupportedGamesIds);
                if (hostGames.Count() != options.SupportedGamesIds.Length)
                {
                    throw new ValidationException("Some of supported games ids is invalid");
                }

                host.SupportedGames = new List<Game>();
                foreach (var game in hostGames)
                {
                    host.SupportedGames.Add(game);
                }
            }

            _gameHostRepository.Add(host);
            _unitOfWork.Commit();

            return host;
        }

        private void ValidateHostEntity(GameHost host)
        {
            if (host.ServerAddress == null)
            {
                throw new ValidationException("Game host address cannot be NULL");
            }
            if (host.UserName == null)
            {
                throw new ValidationException("Game host username cannot be NULL");
            }
            if (host.Password == null)
            {
                throw new ValidationException("Game host password cannot be NULL");
            }
        }
    }
}