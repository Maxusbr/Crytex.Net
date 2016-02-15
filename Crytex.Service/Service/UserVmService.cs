using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq.Expressions;
using Crytex.Service.Extension;
using Crytex.Service.Model;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Service.Service
{
    public class UserVmService : IUserVmService
    {
        private IUserVmRepository _userVmRepo;
        private IUnitOfWork _unitOfWork;
        private readonly IOperatingSystemsService _operatingSystemService;

        public UserVmService(IUserVmRepository userVmRepo, IOperatingSystemsService operatingSystemService, IUnitOfWork unitOfWork)
        {
            this._userVmRepo = userVmRepo;
            this._operatingSystemService = operatingSystemService;
            this._unitOfWork = unitOfWork;
        }


        public virtual UserVm GetVmById(Guid id)
        {
            var vm = this._userVmRepo.Get(v=>v.Id == id, i=>i.OperatingSystem,i=>i.OperatingSystem.ImageFileDescriptor, i => i.IpAdresses, i => i.User);
            if (vm == null)
            {
                throw new InvalidIdentifierException(string.Format("UserVm with id={0} doesnt exist.", id));
            }

            return vm;
        }

        public IEnumerable<UserVm> GetAllVmsHyperV()
        {
           
            return _userVmRepo.GetMany(x=>x.VirtualizationType == TypeVirtualization.HyperV);
        }

        public IEnumerable<UserVm> GetAllVmsVmWare()
        {
        
            return _userVmRepo.GetMany(x => x.VirtualizationType == TypeVirtualization.VmWare);
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

        public IPagedList<UserVm> GetPage(int pageNumber, int pageSize, UserVmSearchParams searchParams = null)
        {
            var page = new PageInfo(pageNumber, pageSize);

            Expression<Func<UserVm, bool>> where = x => true;

            if (searchParams != null)
            {
                if (searchParams.UserId != null)
                {
                    where = where.And(x => x.UserId == searchParams.UserId);
                }
                if (searchParams.Virtualization != null)
                {
                    where = where.And(x => x.VirtualizationType == searchParams.Virtualization);
                }
                if (searchParams.CreateDateFrom != null)
                {
                    where = where.And(x => x.CreateDate >= searchParams.CreateDateFrom);
                }
                if (searchParams.CreateDateTo != null)
                {
                    where = where.And(x => x.CreateDate <= searchParams.CreateDateTo);
                }
            }

            var list = this._userVmRepo.GetPage(page, where, x => x.CreateDate, false, x => x.User, x => x.IpAdresses);
            return list;
        }

        public IPagedList<UserVm> GetPage(int pageNumber, int pageSize, string userId)
        {
    
            var page = new PageInfo(pageNumber, pageSize);
       
            var list = this._userVmRepo.GetPage(page, x => x.UserId == userId, x => x.CreateDate, false, x => x.User, x => x.IpAdresses);
            return list;
        }

        public Guid CreateVm(UserVm userVm)
        {
            var os = this._operatingSystemService.GetById(userVm.OperatingSystemId);
            this.CheckOsHardwareMinRequirements(userVm.GetVmHardwareConfiguration(), os);

            this._userVmRepo.Add(userVm);
            this._unitOfWork.Commit();

            return userVm.Id;
        }

        public void CheckOsHardwareMinRequirements(VmHardwareConfig hardwareConfig, int operatingSystemId)
        {
            var os = this._operatingSystemService.GetById(operatingSystemId);

            this.CheckOsHardwareMinRequirements(hardwareConfig, os);
        }

        public void CheckOsHardwareMinRequirements(VmHardwareConfig hardwareConfig, OperatingSystem os)
        {
            if(hardwareConfig.Cpu < os.MinCoreCount)
            {
                throw new ValidationException($"Cannot create vm with CoreCount={hardwareConfig.Cpu}."+ 
                    $"Min CoreCount for this OS is {os.MinCoreCount}");
            }
            if (hardwareConfig.RamMB < os.MinRamCount)
            {
                throw new ValidationException($"Cannot create vm with RamCount={hardwareConfig.RamMB}." +
                    $"Min RamCount for this OS is {os.MinRamCount}");
            }
            if (hardwareConfig.HardDriveSizeGB < os.MinHardDriveSize)
            {
                throw new ValidationException($"Cannot create vm with HardDriveSize={hardwareConfig.HardDriveSizeGB}." +
                    $"Min HardDriveSize for this OS is {os.MinHardDriveSize}");
            }
        }

        public void UpdateVm(Guid vmId, int? cpu = null, int? hdd = null, int? ram = null)
        {
         
            var userVm = this.GetVmById(vmId);

            // Get vm's VmHardwareConf object, update its props and pass it to CheckOsHardwareMinRequirements
            var hardwareConf = userVm.GetVmHardwareConfiguration();
            hardwareConf.Cpu = cpu ?? userVm.CoreCount;
            hardwareConf.RamMB = ram ?? userVm.RamCount;
            hardwareConf.HardDriveSizeGB = hdd ?? userVm.HardDriveSize;
            this.CheckOsHardwareMinRequirements(hardwareConf, userVm.OperatingSystemId); // Throws exception if new conf is invalid

            // New config is ok. Update UserVm entity
            userVm.CoreCount = cpu ?? userVm.CoreCount;
            userVm.RamCount = ram ?? userVm.RamCount;
            userVm.HardDriveSize = hdd ?? userVm.HardDriveSize;

            this._userVmRepo.Update(userVm);
            this._unitOfWork.Commit();
        }

        public void AddIpAddressesToVm(Guid vmId, IEnumerable<VmIpAddress> addresses)
        {
            var vm = this.GetVmById(vmId);

            foreach (var ip in addresses) { vm.IpAdresses.Add(ip); }

            this._unitOfWork.Commit();
        }

        public void UpdateVm(UserVm vm)
        {
            var userVm = this.GetVmById(vm.Id);

            userVm.CoreCount = vm.CoreCount;
            userVm.RamCount = vm.RamCount;
            userVm.HardDriveSize = vm.HardDriveSize;
            userVm.CreateDate = vm.CreateDate;
            userVm.HyperVHostId = vm.HyperVHostId;
            userVm.Name = vm.Name;
            userVm.OperatingSystemPassword = vm.OperatingSystemPassword;
            userVm.OperatingSystemId = vm.OperatingSystemId;
            userVm.Status = vm.Status;
            userVm.UserId = vm.UserId;
            userVm.VirtualizationType = vm.VirtualizationType;
            userVm.VmWareCenterId = vm.VmWareCenterId;

            this._userVmRepo.Update(userVm);
            this._unitOfWork.Commit();
        }

        public void UpdateVmStatus(Guid vmId, TypeChangeStatus status)
        {
            
            var userVm = this.GetVmById(vmId);

           
            switch (status)
            {
                case TypeChangeStatus.Start:
                    userVm.Status = StatusVM.Enable;
                    break;
                case TypeChangeStatus.Reload:
                    userVm.Status = StatusVM.Enable;
                    break;
                case TypeChangeStatus.PowerOff:
                    userVm.Status = StatusVM.Disable;
                    break;
                case TypeChangeStatus.Stop:
                    userVm.Status = StatusVM.Disable;
                    break;
            }

            this._userVmRepo.Update(userVm);
            this._unitOfWork.Commit();
        }

        public void MarkAsDeleted(Guid vmId)
        {
            var vm = this.GetVmById(vmId);
            vm.Status = StatusVM.Deleted;
            this._userVmRepo.Update(vm);
            this._unitOfWork.Commit();
        }

        public IEnumerable<UserVm> GetAllVmsByUserId(string userId)
        {
            var vms = this._userVmRepo.GetMany(vm => vm.UserId == userId);

            return vms;
        }
    }
}
