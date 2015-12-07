using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Service.IService;
using System.Security.Principal;
using Crytex.Model.Models;
using System;
using Microsoft.AspNet.Identity;
using Crytex.Model.Exceptions;
using PagedList;
using System.Linq.Expressions;
using Crytex.Service.Extension;

namespace Crytex.Service.Service.SecureService
{
    public class SecureVmBackupService : VmBackupService, IVmBackupService
    {
        private readonly IIdentity _userIdentity;

        public SecureVmBackupService(IVmBackupRepository backupRepo, IUnitOfWork unitOfWork,
            IIdentity userIdentity) : base(backupRepo, unitOfWork)
        {
            this._userIdentity = userIdentity;
        }

        public override VmBackup GetById(Guid guid)
        {
            var backup =  base.GetById(guid);

            if(backup.Vm.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access denied for backup with id={guid.ToString()}");
            }

            return backup;
        }

        public override IPagedList<VmBackup> GetPage(int pageNumber, int pageSize, DateTime? from, DateTime? to)
        {
            var userId = this._userIdentity.GetUserId();
            Expression<Func<VmBackup, bool>> where = x => x.Vm.UserId == userId;
            if (from != null)
            {
                where = where.And(x => x.DateCreated >= from);
            }
            if (to != null)
            {
                where = where.And(x => x.DateCreated <= to);
            }

            var page = this.GetPage(pageNumber, pageSize, where);

            return page;
        }
    }
}
