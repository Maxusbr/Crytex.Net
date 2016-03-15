using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.GameServers;
using Crytex.Service.IService;
using Crytex.Service.Model;
using PagedList;

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
                GameServersMaxCount = options.GameServersMaxCount,
                GameServersCount = 0
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