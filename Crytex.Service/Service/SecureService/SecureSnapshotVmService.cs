using System;
using System.Security.Principal;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Microsoft.AspNet.Identity;

namespace Crytex.Service.Service.SecureService
{
    public class SecureSnapshotVmService : SnapshotVmService, ISnapshotVmService
    {
        private readonly IIdentity _userIdentity;

        public SecureSnapshotVmService(ISnapshotVmRepository snapshotVmRepository, ITaskV2Service taskService, IUserVmService userVmService,
            IUnitOfWork unitOfWork, IIdentity userIdentity) : base(snapshotVmRepository, taskService, userVmService, unitOfWork)
        {
            _userIdentity = userIdentity;
        }

        public override SnapshotVm Create(SnapshotVm newSnapShot)
        {
            var vm = _userVmService.GetVmById(newSnapShot.VmId);
            if (vm.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException(
                    $"Access denied for SubscriptionVm with id={vm.Id}");
            }

            return base.Create(newSnapShot);
        }

        public override SnapshotVm GetById(Guid snapshotId)
        {
            var snapshot = base.GetById(snapshotId);
            if (snapshot.Vm.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException(
                    $"Access denied for SubscriptionVm with id={snapshot.Vm.Id}");
            }

            return snapshot;
        }
    }
}
