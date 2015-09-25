using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Exceptions;

namespace Crytex.Service.Service
{
    public class SystemCenterVirtualManagerService : ISystemCenterVirtualManagerService
    {
        private ISystemCenterVirtualManagerRepository _systemCenterVirtualManagerRepo;
        private IHyperVHostRepository _hyperVHostRepo;
        private IHyperVHostResourceRepository _hyperVHostResourceRepo;
        private IUnitOfWork _unitOfWork;

        public SystemCenterVirtualManagerService(IUnitOfWork unitOfWork, ISystemCenterVirtualManagerRepository managerRepo,
            IHyperVHostRepository hyperVHostRepo, IHyperVHostResourceRepository hyperVHostResourceRepo)
        {
            this._systemCenterVirtualManagerRepo = managerRepo;
            this._hyperVHostRepo = hyperVHostRepo;
            this._hyperVHostResourceRepo = hyperVHostResourceRepo;
            this._unitOfWork = unitOfWork;
        }

        public SystemCenterVirtualManager Create(SystemCenterVirtualManager manager)
        {
            this._systemCenterVirtualManagerRepo.Add(manager);
            
            this._unitOfWork.Commit();
            return manager;
        }

        public void Update(String id,SystemCenterVirtualManager updateManager)
        {
            var guid = new Guid(id);
            var manager = this._systemCenterVirtualManagerRepo.GetById(guid);

            if (manager == null)
            {
                throw new InvalidIdentifierException(string.Format("UpdateManager width Id={0} doesn't exists", guid));
            }

            manager.Host = updateManager.Host;
            manager.Name = updateManager.Name;
            manager.UserName = updateManager.UserName;
            manager.Password = updateManager.Password;
    
            this._systemCenterVirtualManagerRepo.Update(manager);
            this._unitOfWork.Commit();
        }

        public IEnumerable<HyperVHost> GetAllHyperVHosts()
        {
            return _hyperVHostRepo.GetMany(h=>!h.Deleted);
        }
        public SystemCenterVirtualManager GetById(string id)
        {
            var guid = new Guid(id);
            var manager = this._systemCenterVirtualManagerRepo.Get(m => m.Id == guid,
                x => x.HyperVHosts,
                x => x.HyperVHosts.Select(h => h.Resources));

            if (manager == null)
            {
                throw new InvalidIdentifierException(string.Format(@"SystemCenterVirtualManager with id={0}
                                                                        doesn't exist", id));
            }

            return manager;
        }


        public void Delete(string id)
        {
            var guid = new Guid(id);
            var manager = this._systemCenterVirtualManagerRepo.Get(x => x.Id == guid, 
                x => x.HyperVHosts,
                x => x.HyperVHosts.Select(h => h.Resources));

            if (manager == null)
            {
                throw new InvalidIdentifierException(string.Format(@"SystemCenterVirtualManager with id={0} 
                                                                            doesn't exist", id));
            }

            foreach (var host in manager.HyperVHosts)
            {
                host.Deleted = true;
                foreach (var resource in host.Resources)
                {
                    resource.Deleted = true;
                }
            }
            manager.Deleted = true;

            this._systemCenterVirtualManagerRepo.Update(manager);
            this._unitOfWork.Commit();
        }


        public IEnumerable<SystemCenterVirtualManager> GetAll(bool includeHosts = true)
        {
            IEnumerable<SystemCenterVirtualManager> managers;
            if (includeHosts)
            {
                managers = this._systemCenterVirtualManagerRepo.GetAll(x => x.HyperVHosts,
                x => x.HyperVHosts.Select(h => h.Resources));
            }
            else
            {
                managers = this._systemCenterVirtualManagerRepo.GetAll();
            }
            
            return managers;
        }




        public void UpdateHyperVHost(Guid guid, HyperVHost host)
        {

            var hostToUpdate = this._hyperVHostRepo.GetById(guid);

            if (hostToUpdate == null)
            {
                throw new InvalidIdentifierException(string.Format(@"HyperVHost with id = {0} doesn't exist", guid.ToString()));
            }

            hostToUpdate.Host = host.Host;
            hostToUpdate.UserName = host.UserName;
            hostToUpdate.Password = host.Password;
            hostToUpdate.RamSize = host.RamSize;
            hostToUpdate.Valid = host.Valid;

            this._hyperVHostRepo.Update(hostToUpdate);
            this._unitOfWork.Commit();
        }


        public HyperVHost AddHyperVHost(HyperVHost remoteHost)
        {
            this._hyperVHostRepo.Add(remoteHost);
            
            return remoteHost;
        }


        public HyperVHostResource UpdateHyperVHostResource(Guid guid, HyperVHostResource resource)
        {
            var resourceToUpdate = this._hyperVHostResourceRepo.GetById(guid);
            if (resourceToUpdate == null)
            {
                throw new InvalidIdentifierException(string.Format("HyperVHostResource with id={0} doesn't exist"));
            }

            resourceToUpdate.UpdateDate = resource.UpdateDate;
            resourceToUpdate.Valid = resourceToUpdate.Valid;
            resourceToUpdate.Value = resourceToUpdate.Value;

            this._hyperVHostResourceRepo.Update(resourceToUpdate);
            this._unitOfWork.Commit();

            return resourceToUpdate;
        }


        public HyperVHostResource AddHyperVHostResource(HyperVHostResource resource)
        {
            this._hyperVHostResourceRepo.Add(resource);
            return resource;
        }
    }
}
