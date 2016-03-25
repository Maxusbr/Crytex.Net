using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models.GameServers;
using Crytex.Service.IService;
using Crytex.Service.Model;
using PagedList;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Crytex.Service.Service
{
    public class GameHostService : IGameHostService
    {
        private readonly IGameService _gameSerice;
        private readonly IGameHostRepository _gameHostRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocationService _locationService;

        public GameHostService(IGameService gameSerice, IGameHostRepository gameHostRepository, IUnitOfWork unitOfWork,
            ILocationService locationService)
        {
            _gameSerice = gameSerice;
            _gameHostRepository = gameHostRepository;
            _unitOfWork = unitOfWork;
            _locationService = locationService;
        }


        public GameHost GetById(int id)
        {
            var host = _gameHostRepository.Get(x => x.Id == id, x => x.GameServers);

            if (host == null)
            {
                throw new InvalidIdentifierException($"GameHost with id={id} doesnt exist");
            }

            return host;
        }

        public GameHost Create(GameHostCreateOptions options)
        {
            var host = new GameHost
            {
                ServerAddress = options.ServerAddress,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password,
                GameServersMaxCount = options.GameServersMaxCount,
                GameServersCount = 0,
                LocationId = options.LocationId
            };

            this.ValidateHostEntity(host);

            var hostLocation = _locationService.GetById(options.LocationId);
            host.Location = hostLocation;
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

        public GameHost GetGameHostWithAvalailableSlot(int gameId)
        {
            Expression<Func<GameHost, bool>> where = x => x.GameServersCount < x.GameServersMaxCount && x.SupportedGames.Any(y => y.Id == gameId);
            var hosts = _gameHostRepository.GetMany(where);

            return hosts.FirstOrDefault();
        }

        public void Update(Int32 id, GameHostCreateOptions option)
        {
            var host = _gameHostRepository.Get(x => x.Id == id, x => x.SupportedGames);
            if (host == null)
            {
                throw new ValidationException($"GameHost with Id={id} not found");
            }
            if (option.GameServersMaxCount != 0)
                host.GameServersMaxCount = option.GameServersMaxCount;
            if (option.Port != 0)
                host.Port = option.Port;
            if (!string.IsNullOrEmpty(option.ServerAddress))
                host.ServerAddress = option.ServerAddress;
            if (!string.IsNullOrEmpty(option.UserName))
                host.UserName = option.UserName;
            if (!string.IsNullOrEmpty(option.Password))
                host.Password = option.Password;
            var hostLocation = _locationService.GetById(option.LocationId);
            host.Location = hostLocation;
            if (option.SupportedGamesIds != null && option.SupportedGamesIds.Any())
            {
                var hostGames = _gameSerice.GetGamesByIds(option.SupportedGamesIds);
                if (hostGames.Count() != option.SupportedGamesIds.Length)
                {
                    throw new ValidationException("Some of supported games ids is invalid");
                }
                if(host.SupportedGames == null) host.SupportedGames = new List<Game>();
                host.SupportedGames.Clear();
                foreach (var game in hostGames)
                    host.SupportedGames.Add(game);
            }

            _gameHostRepository.Update(host);
            _unitOfWork.Commit();
        }

        public void Delete(Int32 id)
        {
            var host = _gameHostRepository.Get(x => x.Id == id, x => x.SupportedGames);
            if (host == null)
            {
                throw new ValidationException($"GameHost with Id={id} not found");
            }

            _gameHostRepository.Delete(host);
            _unitOfWork.Commit();
        }

        public IPagedList<GameHost> GetPage(int pageNumber, int pageSize)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<GameHost, bool>> where = x => true;

            var pagedList = _gameHostRepository.GetPage(pageInfo, where, x => x.Id, false, x => x.SupportedGames);

            return pagedList;
        }

        public int GetFreePort(int id)
        {
            var host = GetById(id);
            var gameServersSortedByPort = host.GameServers.OrderBy(gs => gs.FirstPortInRange).ToList();

            var newPort = 1000;
            foreach (var gameServer in gameServersSortedByPort)
            {
                if (newPort >= gameServer.FirstPortInRange &&
                    newPort <= gameServer.FirstPortInRange + gameServer.PortRangeSize - 1)
                {
                    newPort = gameServer.FirstPortInRange + gameServer.PortRangeSize;
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (newPort > 65000)
            {
                throw new ApplicationException("no ports available");
            }

            return newPort;
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