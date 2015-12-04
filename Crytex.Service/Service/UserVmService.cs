using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;

namespace Crytex.Service.Service
{
    public class UserVmService : IUserVmService
    {
        private IUserVmRepository _userVmRepo;
        private IUnitOfWork _unitOfWork;

        public UserVmService(IUserVmRepository userVmRepo, IUnitOfWork unitOfWork)
        {
            this._userVmRepo = userVmRepo;
            this._unitOfWork = unitOfWork;
        }

        public UserVm GetVmById(Guid id)
        {
            var vm = this._userVmRepo.Get(v=>v.Id == id, i=>i.ServerTemplate);
            if (vm == null)
            {
                throw new InvalidIdentifierException(string.Format("UserVm with id={0} doesnt exist.", id));
            }

            return vm;
        }

        public IEnumerable<UserVm> GetAllVmsHyperV()
        {
            return _userVmRepo.GetMany(x=>x.VurtualizationType == TypeVirtualization.HyperV);
        }

        public IEnumerable<UserVm> GetAllVmsVmWare()
        {
            return _userVmRepo.GetMany(x => x.VurtualizationType == TypeVirtualization.VmWare);
        }

        public IEnumerable<UserVm> GetVmByListId(List<Guid> listId)
        {
            
            var vms = this._userVmRepo.GetMany(v => listId.Contains(v.Id));
            if (vms == null)
            {
                throw new ObjectNotFoundException();
            }

            return vms;
        }

        public IPagedList<UserVm> GetPage(int pageNumber, int pageSize, string userId)
        {
            var page = new PageInfo(pageNumber, pageSize);
            var list = this._userVmRepo.GetPage(page, x => x.UserId == userId, x => x.Id);
            return list;
        }

        public Guid CreateVm(UserVm userVm)
        {
            if (userVm.VurtualizationType == TypeVirtualization.HyperV && userVm.HyperVHostId == null)
            {
                throw new ApplicationException("HyperVHostId property value is required for HyperV virtualization type");
            }
            if (userVm.VurtualizationType == TypeVirtualization.VmWare && userVm.VmWareCenterId == null)
            {
                throw new ApplicationException("VmWareCenterId property value is required for VmWare virtualization type");
            }

            this._userVmRepo.Add(userVm);
            this._unitOfWork.Commit();

            return userVm.Id;
        }

        public void UpdateVm(Guid vmId, int? cpu = null, int? hdd = null, int? ram = null)
        {
            var userVm = this._userVmRepo.GetById(vmId);
            
            if (userVm == null)
            {
                throw new InvalidIdentifierException(string.Format("UserVm with Id = {0} doesnt exist.",vmId));
            }

            userVm.CoreCount = cpu ?? userVm.CoreCount;
            userVm.RamCount = ram ?? userVm.RamCount;
            userVm.HardDriveSize = hdd ?? userVm.HardDriveSize;

            this._userVmRepo.Update(userVm);
            this._unitOfWork.Commit();
        }

        public void UpdateVmStatus(Guid vmId, TypeChangeStatus status)
        {
            var userVm = this._userVmRepo.GetById(vmId);

            if (userVm == null)
            {
                throw new InvalidIdentifierException(string.Format("UserVm with Id = {0} doesnt exist.", vmId));
            }

            switch (status)
            {
                case TypeChangeStatus.Start:
                    userVm.Status = StatusVM.Enable;
                    break;
                case TypeChangeStatus.Reload:
                    userVm.Status = StatusVM.Enable;
                    break;
                case TypeChangeStatus.PowerOf:
                    userVm.Status = StatusVM.Disable;
                    break;
                case TypeChangeStatus.Stop:
                    userVm.Status = StatusVM.Disable;
                    break;
            }

            this._userVmRepo.Update(userVm);
            this._unitOfWork.Commit();
        }
    }
}
