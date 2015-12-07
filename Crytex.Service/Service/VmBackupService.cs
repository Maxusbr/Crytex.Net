using System;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Model.Exceptions;
using PagedList;
using Crytex.Data.Infrastructure;
using System.Linq.Expressions;
using Crytex.Service.Extension;

namespace Crytex.Service.Service
{
    public class VmBackupService : IVmBackupService
    {
        private readonly IVmBackupRepository _backupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VmBackupService(IVmBackupRepository backupRepo, IUnitOfWork unitOfWork)
        {
            this._backupRepository = backupRepo;
            this._unitOfWork = unitOfWork;
        }

        public void Create(VmBackup newBackupDbEntity)
        {
            this._backupRepository.Add(newBackupDbEntity);
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
