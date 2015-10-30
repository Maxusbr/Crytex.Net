using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this._hyperVRepo.Add(hyperVHost);
            this._unitOfWork.Commit();

            return hyperVHost;
        }


        public HyperVHost GetHyperVById(int id)
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
            var hyperVHostToUpdate = this._hyperVRepo.GetById(hyperVHost.Id);

            if (hyperVHostToUpdate == null)
            {
                throw new InvalidIdentifierException(string.Format("hyperVHost with id={0} doesnt exist", hyperVHost.Id));
            }

            hyperVHostToUpdate.Host = hyperVHost.Host;
            hyperVHostToUpdate.UserName = hyperVHost.UserName;
            hyperVHostToUpdate.Password = hyperVHost.Password;


            this._hyperVRepo.Update(hyperVHostToUpdate);
            this._unitOfWork.Commit();
        }
    }
}
