using System;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Model.Exceptions;
using PagedList;
using Crytex.Data.Infrastructure;
using System.Linq.Expressions;
using Crytex.Service.Extension;
using System.Collections.Generic;

namespace Crytex.Service.Service
{
    public class VmBackupService : IVmBackupService
    {
        private readonly IVmBackupRepository _backupRepository;
        private readonly ITaskV2Service _taskService;
        private readonly ISubscriptionVmService _subscriptionVmService;
        private readonly IUnitOfWork _unitOfWork;

        public VmBackupService(IVmBackupRepository backupRepo, ITaskV2Service taskService, ISubscriptionVmService subscriptionVmService,
            IUnitOfWork unitOfWork)
        {
            this._backupRepository = backupRepo;
            _taskService = taskService;
            _subscriptionVmService = subscriptionVmService;
            this._unitOfWork = unitOfWork;
        }

        public VmBackup Create(Guid subscriptionVmId, string name)
        {
            var sub = _subscriptionVmService.GetById(subscriptionVmId);

            // create backup task
            var backupTask = new TaskV2
            {
                ResourceId = sub.Id,
                ResourceType = ResourceType.SubscriptionVm,
                TypeTask = TypeTask.Backup,
                Virtualization = sub.UserVm.VirtualizationType,
                UserId = sub.UserId
            };
            var backupTaskOptions = new BackupOptions
            {
                BackupName = name,
                VmId = sub.UserVm.Id
            };
            this._taskService.CreateTask(backupTask, backupTaskOptions);

            var newBackup = new VmBackup
            {
                Name = name,
                VmId = sub.UserVm.Id
            };
            newBackup.DateCreated = DateTime.UtcNow;
            newBackup.Status = VmBackupStatus.Creting;

            this._backupRepository.Add(newBackup);
            this._unitOfWork.Commit();

            return newBackup;
        }

        public void Delete(Guid vmBackupId)
        {
            var backup = this._backupRepository.GetById(vmBackupId);
            backup.Status = VmBackupStatus.Deleted;

            var sub = _subscriptionVmService.GetById(backup.VmId);

            // delete backup
            var deleteBackupTask = new TaskV2
            {
                ResourceId = sub.Id,
                ResourceType = ResourceType.SubscriptionVm,
                TypeTask = TypeTask.DeleteBackup,
                Virtualization = sub.UserVm.VirtualizationType,
                UserId = sub.UserId
            };
            var deleteBackupTaskOptions = new DeleteBackupOptions
            {
                VmBackupId = backup.Id
            };
            this._taskService.CreateTask(deleteBackupTask, deleteBackupTaskOptions);

            
            this._backupRepository.Update(backup);
            this._unitOfWork.Commit();
        }

        public void DeleteBackupDbEntity(Guid backupGuid)
        {
            var backup = this._backupRepository.GetById(backupGuid);
            this._backupRepository.Delete(backup);
            this._unitOfWork.Commit();
        }

        public virtual VmBackup GetById(Guid guid)
        {
            var backup = this._backupRepository.Get(b => b.Id == guid, b => b.Vm);

            if (backup == null)
            {
                throw new InvalidIdentifierException($"Backup with id={guid.ToString()} doesn't exist");
            }

            return backup;
        }

        public IEnumerable<VmBackup> GetByVmId(Guid id)
        {
            var backups = this._backupRepository.GetMany(x => x.VmId == id);

            return backups;
        }

        public virtual IPagedList<VmBackup> GetPage(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null, Guid? vmId = null)
        {
            Expression<Func<VmBackup, bool>> where = x => true;
            if (from != null)
            {
                where = where.And(x => x.DateCreated >= from);
            }
            if (to != null)
            {
                where = where.And(x => x.DateCreated <= to);
            }
            if (vmId != null)
            {
                where = where.And(x => x.VmId == vmId);
            }

            var page = this.GetPage(pageNumber, pageSize, where);

            return page;
        }

        public void UpdateBackupStatus(Guid backupGuid, VmBackupStatus status)
        {
            var backup = this._backupRepository.GetById(backupGuid);
            backup.Status = status;

            this._backupRepository.Update(backup);
            this._unitOfWork.Commit();
        }

        protected IPagedList<VmBackup> GetPage(int pageNumber, int pageSize, Expression<Func<VmBackup, bool>> where)
        {
            var pageInfo = new PageInfo
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var page = this._backupRepository.GetPage(pageInfo, where, b => b.DateCreated);

            return page;
        }
    }
}
