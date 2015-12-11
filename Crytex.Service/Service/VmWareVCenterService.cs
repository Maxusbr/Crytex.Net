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
    public class VmWareVCenterService : IVmWareVCenterService
    {
        private IVmWareVCenterRepository _vCenterRepo;
        private IUnitOfWork _unitOfWork;

        public VmWareVCenterService(IVmWareVCenterRepository vCenterRepo, IUnitOfWork unitOfWork)
        {
            this._vCenterRepo = vCenterRepo;
            this._unitOfWork = unitOfWork;
        }

        public VmWareVCenter CreateVCenter(VmWareVCenter vCenter)
        {
            this._vCenterRepo.Add(vCenter);
            this._unitOfWork.Commit();

            return vCenter;
        }


        public VmWareVCenter GetVCenterById(Guid id)
        {
            var vCenter = this._vCenterRepo.GetById(id);

            if (vCenter == null)
            {
                throw new InvalidIdentifierException(string.Format("VCenter with id={0} doesnt exist", id));
            }

            return vCenter;
        }

        public IEnumerable<VmWareVCenter> GetAllVCenters()
        {
            var vCenters = this._vCenterRepo.GetAll();

            return vCenters;
        }

        public void UpdateVCenter(Guid id, VmWareVCenter vCenter)
        {
            var vCenterToUpdate = this._vCenterRepo.GetById(id);

            if (vCenterToUpdate == null)
            {
                throw new InvalidIdentifierException(string.Format("VCenter with id={0} doesnt exist", id));
            }

            vCenterToUpdate.Name = vCenter.Name;
            vCenterToUpdate.UserName = vCenter.UserName;
            vCenterToUpdate.Password = vCenter.Password;
            vCenterToUpdate.ServerAddress = vCenter.ServerAddress;

            this._vCenterRepo.Update(vCenterToUpdate);
            this._unitOfWork.Commit();
        }
    }
}
