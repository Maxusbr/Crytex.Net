using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Enums;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.Service
{
    public class PhysicalServerService : IPhysicalServerService
    {
        private readonly IBoughtPhysicalServerOptionRepository _boughtOptionRepository;
        private readonly IBoughtPhysicalServerRepository _boughtServerRepository;
        private readonly IPhysicalServerOptionRepository _optionRepository;
        private readonly IPhysicalServerOptionsAvailableRepository _availableOptionRepository;
        private readonly IPhysicalServerRepository _serverRepository;
        private readonly IUnitOfWork _uniOfWork;

        public PhysicalServerService(IBoughtPhysicalServerOptionRepository boughtOptionRepository,
            IBoughtPhysicalServerRepository boughtServerRepository, IPhysicalServerOptionRepository optionRepository,
            IPhysicalServerOptionsAvailableRepository availableOptionRepository, IPhysicalServerRepository serverRepository, IUnitOfWork uniOfWork)
        {
            _boughtOptionRepository = boughtOptionRepository;
            _boughtServerRepository = boughtServerRepository;
            _optionRepository = optionRepository;
            _availableOptionRepository = availableOptionRepository;
            _serverRepository = serverRepository;
            _uniOfWork = uniOfWork;
        }

        /// <summary>
        /// Создание конфигурации физического сервера
        /// </summary>
        /// <param name="serverParam"></param>
        /// <returns></returns>
        public PhysicalServer CreatePhysicalServer(CreatePhysicalServerParam serverParam)
        {
            var server = new PhysicalServer { ProcessorName = serverParam.ProcessorName, Description = serverParam.Description };
            if (serverParam.CalculatePrice && serverParam.ServerOptions != null)
                server.Price = serverParam.ServerOptions.Where(o => o.IsDefault).Sum(s => s.Price);
            else
                server.Price = serverParam.Price;
            _serverRepository.Add(server);
            _uniOfWork.Commit();

            if (serverParam.ServerOptions != null)
                AddOptionsAviable(server.Id, serverParam.ServerOptions);
            return server;
        }

        /// <summary>
        /// Обновление доступных опций для физического сервера. 
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="optionsParams"></param>
        public void UpdateOptionsAviable(Guid serverId, IEnumerable<PhysicalServerOptionsParams> optionsParams)
        {
            var server = _serverRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            _availableOptionRepository.Delete(x => x.PhysicalServerId == serverId);
            AddOptionsAviable(serverId, optionsParams);
        }

        /// <summary>
        /// Добавление доступных опций для физического сервера. 
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="optionsParams"></param>
        public void AddOptionsAviable(Guid serverId, IEnumerable<PhysicalServerOptionsParams> optionsParams)
        {
            var server = _serverRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            foreach (var opt in optionsParams)
            {
                var option = CreateOrUpdateOptions(opt);
                var aviable = _availableOptionRepository.Get(x => x.PhysicalServerId == serverId && x.OptionId == option.Id,
                        x => x.Option, x => x.Server);
                if (aviable == null)
                    _availableOptionRepository.Add(new PhysicalServerOptionsAvailable
                    {
                        OptionId = option.Id,
                        PhysicalServerId = serverId,
                        IsDefault = opt.IsDefault
                    });
                else
                {
                    aviable.IsDefault = opt.IsDefault;
                    _availableOptionRepository.Update(aviable);
                }
            }
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Создание/изменение опции для физических серверов
        /// </summary>
        /// <param name="optionsParams"></param>
        /// <returns></returns>
        public PhysicalServerOption CreateOrUpdateOptions(PhysicalServerOptionsParams optionsParams)
        {
            PhysicalServerOption option;
            if (optionsParams.OptionId != null)
            {
                option = _optionRepository.Get(x => x.Id == optionsParams.OptionId);
                if (option == null)
                {
                    throw new InvalidIdentifierException($"PhysicalServerOption with id={optionsParams.OptionId} doesn't exist");
                }
                option.Description = optionsParams.Description;
                option.Name = optionsParams.Name;
                option.Price = optionsParams.Price;
                option.Type = optionsParams.Type;
                _optionRepository.Update(option);
            }
            else
            {
                option = new PhysicalServerOption
                {
                    Name = optionsParams.Name,
                    Description = optionsParams.Description,
                    Price = optionsParams.Price,
                    Type = optionsParams.Type
                };
                _optionRepository.Add(option);
            }
            _uniOfWork.Commit();
            return option;
        }

        /// <summary>
        /// Создание/изменение опций для физических серверов
        /// </summary>
        /// <param name="optionsParams"></param>
        public void CreateOrUpdateOptions(IEnumerable<PhysicalServerOptionsParams> optionsParams)
        {
            foreach (var el in optionsParams)
                CreateOrUpdateOptions(el);
        }

        /// <summary>
        /// Удаление конфигурации физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        public void DeletePhysicalServer(Guid serverId)
        {
            var server = _serverRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            _serverRepository.Delete(server);
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Удаление опции для физического сервера
        /// </summary>
        /// <param name="optionId"></param>
        public void DeletePhysicalServerOption(Guid optionId)
        {
            var option = _optionRepository.Get(x => x.Id == optionId);
            if (option == null)
            {
                throw new InvalidIdentifierException($"PhysicalServerOption with id={optionId} doesn't exist");
            }
            _optionRepository.Delete(option);
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Покупка физического сервера
        /// </summary>
        /// <param name="serverParam"></param>
        /// <returns></returns>
        public BoughtPhysicalServer BuyPhysicalServer(BuyPhysicalServerParam serverParam)
        {
            var serverConfig = _serverRepository.GetById(serverParam.PhysicalServerId);
            if (serverConfig == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverParam.PhysicalServerId} doesn't exist");
            }
            var server = new BoughtPhysicalServer
            {
                PhysicalServerId = serverConfig.Id,
                CreateDate = DateTime.UtcNow,
                Status = BoughtPhysicalServerStatus.Creting,
                CountMonth = serverParam.CountMonth,
                DiscountPrice = serverParam.DiscountPrice,
                UserId = serverParam.UserId
            };
            _boughtServerRepository.Add(server);
            _uniOfWork.Commit();

            AddOptionToBoughtPhysicalServer(server.Id, serverParam.OptionIds);

            return GetBoughtPhysicalServer(server.Id);
        }

        private void AddOptionToBoughtPhysicalServer(Guid serverId, IEnumerable<Guid> options)
        {
            foreach (var guid in options)
            {
                var option = _optionRepository.GetById(guid);
                if (option == null)
                {
                    throw new InvalidIdentifierException($"PhysicalServerOption with id={guid} doesn't exist");
                }
                var boughtOption = new BoughtPhysicalServerOption { BoughtPhysicalServerId = serverId, OptionId = option.Id };
                _boughtOptionRepository.Add(boughtOption);
            }
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Обновление состояния купленного физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="state"></param>
        public void UpdateBoughtPhysicalServerState(Guid serverId, BoughtPhysicalServerStatus state)
        {
            var server = _boughtServerRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            server.Status = state;
            _boughtServerRepository.Update(server);
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Обновление конфигурации купленного физического сервера
        /// </summary>
        /// <param name="serverParam"></param>
        /// <returns></returns>
        public BoughtPhysicalServer UpdateBoughtPhysicalServer(UpdatePhysicalServerParam serverParam)
        {
            var server = _boughtServerRepository.Get(x => x.Id == serverParam.ServerId, x => x.ServerOption,
                x => x.Server, x => x.User);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverParam.ServerId} doesn't exist");
            }
            if (serverParam.CountMonth != null)
                server.CountMonth = (int)serverParam.CountMonth;
            if (serverParam.PhysicalServerId != null)
                server.PhysicalServerId = (Guid)serverParam.PhysicalServerId;
            if (serverParam.DiscountPrice != null)
                server.DiscountPrice = (decimal)serverParam.DiscountPrice;
            if (serverParam.OptionIds != null)
            {
                _boughtOptionRepository.Delete(x => x.BoughtPhysicalServerId == server.Id);
                AddOptionToBoughtPhysicalServer(server.Id, serverParam.OptionIds);
            }

            _boughtServerRepository.Update(server);
            _uniOfWork.Commit();

            return GetBoughtPhysicalServer(server.Id);
        }

        public IPagedList<PhysicalServer> GetPagePhysicalServer(int pageNumber, int pageSize, PhysicalServerSearchParams searchParams)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<PhysicalServer, bool>> where = x => true;


            var pagedList = _serverRepository.GetPage(pageInfo, where, x => x.Id, false);
            foreach (var server in pagedList)
            {
                var options = _availableOptionRepository.GetMany(x => x.PhysicalServerId == server.Id, x => x.Option);
                server.AvailableOptions = options.Where(o => o.IsDefault).ToList();
            }
            return pagedList;
        }

        public IPagedList<PhysicalServerOption> GetPagePhysicalServerOption(int pageNumber, int pageSize, PhysicalServerOptionSearchParams searchParams)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<PhysicalServerOption, bool>> where = x => true;


            var pagedList = _optionRepository.GetPage(pageInfo, where, x => x.Id, false);

            return pagedList;
        }

        public IPagedList<BoughtPhysicalServer> GetPageBoughtPhysicalServer(int pageNumber, int pageSize, BoughtPhysicalServerSearchParams searchParams)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<BoughtPhysicalServer, bool>> where = x => true;


            var pagedList = _boughtServerRepository.GetPage(pageInfo, where, x => x.Id, false, x => x.Server, x => x.User);
            foreach (var server in pagedList)
                server.ServerOption = _boughtOptionRepository.GetMany(x => x.BoughtPhysicalServerId == server.Id,
                    x => x.Option, x => x.Server);

            return pagedList;
        }

        /// <summary>
        /// Получить конфигурацию готового физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public PhysicalServer GetReadyPhysicalServer(Guid serverId)
        {
            var server = _serverRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"BoughtPhysicalServer with id={serverId} doesn't exist");
            }
            server.AvailableOptions = new List<PhysicalServerOptionsAvailable>();
            foreach (var option in _availableOptionRepository.GetMany(x => x.PhysicalServerId == serverId, x => x.Option))
                if(option.IsDefault || option.Option.Type == PhysicalServerOptionType.Hdd)
                    server.AvailableOptions.Add(option);

            return server;
        }

        /// <summary>
        /// Получить конфигурацию физического сервера а также список доступных опций
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public PhysicalServer GetAviablePhysicalServer(Guid serverId)
        {
            var server = _serverRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"BoughtPhysicalServer with id={serverId} doesn't exist");
            }
            server.AvailableOptions = _availableOptionRepository.GetMany(x => x.PhysicalServerId == serverId, x => x.Option);

            return server;
        }

        /// <summary>
        /// Получить конфигурацию купленного физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public BoughtPhysicalServer GetBoughtPhysicalServer(Guid serverId)
        {
            var server = _boughtServerRepository.Get(x => x.Id == serverId, x => x.Server, x => x.User);
            if (server == null)
            {
                throw new InvalidIdentifierException($"BoughtPhysicalServer with id={serverId} doesn't exist");
            }

            server.ServerOption = _boughtOptionRepository.GetMany(x => x.BoughtPhysicalServerId == server.Id,
                x => x.Option, x => x.Server);
            return server;
        }
    }
}
