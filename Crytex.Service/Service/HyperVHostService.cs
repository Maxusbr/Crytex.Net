using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;

namespace Crytex.Service.Service
{
    public class HyperVHostService : IHyperVHostService
    {
        private IHyperVHostRepository _hyperVRepo;
        private IUnitOfWork _unitOfWork;

        public HyperVHostService(IHyperVHostRepository hyperVRepo, IUnitOfWork unitOfWork)
        {
            this._hyperVRepo = hyperVRepo;
            this._unitOfWork = unitOfWork;
        }

        public HyperVHost CreateHyperVHost(HyperVHost hyperVHost)
        {
            hyperVHost.DateAdded = DateTime.UtcNow;
            hyperVHost.Disabled = false;
            hyperVHost.Valid = true;

            this._hyperVRepo.Add(hyperVHost);
            this._unitOfWork.Commit();

            return hyperVHost;
        }


        public HyperVHost GetHyperVById(Guid id)
        {
            var hyperVHost = this._hyperVRepo.GetById(id);

            if (hyperVHost == null)
            {
                throw new InvalidIdentifierException(string.Format("hyperVHost with id={0} doesnt exist", id));
            }

            return hyperVHost;
        }

        public IEnumerable<HyperVHost> GetAllHyperVHosts()
        {
            var hyperVHosts = this._hyperVRepo.GetAll();

            return hyperVHosts;
        }

        public void UpdateHyperVHost(HyperVHost hyperVHost)
        {
            var hyperVHostToUpdate = GetHyperVById(hyperVHost.Id);

            hyperVHostToUpdate.Name = hyperVHost.Name;
            hyperVHostToUpdate.Host = hyperVHost.Host;
            hyperVHostToUpdate.CoreNumber = hyperVHost.CoreNumber;
            hyperVHostToUpdate.RamSize = hyperVHost.RamSize;
            hyperVHostToUpdate.UserName = hyperVHost.UserName;
            hyperVHostToUpdate.Password = hyperVHost.Password;
            hyperVHostToUpdate.DefaultVmNetworkName = hyperVHost.DefaultVmNetworkName;
            hyperVHostToUpdate.Disabled = hyperVHost.Disabled;


            this._hyperVRepo.Update(hyperVHostToUpdate);
            this._unitOfWork.Commit();
        }

        public void DeleteHost(Guid id)
        {
            var host = GetHyperVById(id);
            _hyperVRepo.Delete(host);
            _unitOfWork.Commit();
        }
    }
}
