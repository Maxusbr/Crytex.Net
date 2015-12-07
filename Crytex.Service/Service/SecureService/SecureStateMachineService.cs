using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Crytex.Service.Service.SecureService
{
    public class SecureStateMachineService : StateMachineService, IStateMachineService
    {
        private readonly IIdentity _userIdentity;
        private readonly IUserVmRepository _userVmRepo;

        public SecureStateMachineService(IStateMachineRepository stateMachineRepository, IUnitOfWork unitOfWork,
            IIdentity userIdentity, IUserVmRepository userVmRepository) : base(stateMachineRepository, unitOfWork)
        {
            this._userVmRepo = userVmRepository;
            this._userIdentity = userIdentity;
        }

        public override IEnumerable<StateMachine> GetStateByVmId(Guid vmId, int diffInMinutes = 0)
        {
            var vm = this._userVmRepo.GetById(vmId);

            ThrowExceptionIfNeeded(vm);

            return base.GetStateByVmId(vmId, diffInMinutes);
        }

        public override StateMachine GetStateById(int id)
        {
            var state = base.GetStateById(id);
            var vm = this._userVmRepo.GetById(state.VmId);

            ThrowExceptionIfNeeded(vm);

            return state;
        }

        private void ThrowExceptionIfNeeded(UserVm vm)
        {
            if (vm.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access denied for vm with id={vm.Id.ToString()}");
            }
        }
    }
}
