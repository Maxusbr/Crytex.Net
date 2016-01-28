using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Enums;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
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
        public void BuyPhysicalServer(BuyPhysicalServerParam serverParam)
        {
            throw new NotImplementedException();
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

        public IPagedList<PhysicalServer> GetPagePhysicalServer(int pageNumber, int pageSize, PhysicalServerSearchParams searchParams)
        {
            throw new NotImplementedException();
        }

        public IPagedList<PhysicalServerOption> GetPagePhysicalServerOption(int pageNumber, int pageSize, PhysicalServerOptionSearchParams searchParams)
        {
            throw new NotImplementedException();
        }

        public IPagedList<BoughtPhysicalServer> GetPageBoughtPhysicalServer(int pageNumber, int pageSize, BoughtPhysicalServerSearchParams searchParams)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить конфигурацию готового физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public PhysicalServer GetReadyPhysicalServer(Guid serverId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить конфигурацию физического сервера а также список доступных опций
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public PhysicalServer GetAviablePhysicalServer(Guid serverId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить конфигурацию купленного физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public BoughtPhysicalServer GetBoughtPhysicalServer(Guid serverId)
        {
            throw new NotImplementedException();
        }
    }
}
