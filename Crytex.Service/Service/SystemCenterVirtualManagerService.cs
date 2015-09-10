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
        private IUnitOfWork _unitOfWork;

        public SystemCenterVirtualManagerService(IUnitOfWork unitOfWork, ISystemCenterVirtualManagerRepository managerRepo)
        {
            this._systemCenterVirtualManagerRepo = managerRepo;
            this._unitOfWork = unitOfWork;
        }

        public SystemCenterVirtualManager Create(SystemCenterVirtualManager manager)
        {
            this._systemCenterVirtualManagerRepo.Add(manager);
            
            this._unitOfWork.Commit();
            return manager;
        }


        public SystemCenterVirtualManager GetById(string id)
        {
            var guid = new Guid(id);
            var manager = this._systemCenterVirtualManagerRepo.GetById(guid);

            if (manager == null)
            {
                throw new InvalidIdentifierException(string.Format("SystemCenterVirtualManager with id={0} doesn't exist", id));
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
                throw new InvalidIdentifierException(string.Format("SystemCenterVirtualManager with id={0} doesn't exist", id));
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
    }
}
