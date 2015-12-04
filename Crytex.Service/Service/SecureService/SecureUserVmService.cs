using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System.Security.Principal;
using System;
using Microsoft.AspNet.Identity;
using Crytex.Model.Exceptions;

namespace Crytex.Service.Service.SecureService
{
    public class SecureUserVmService : UserVmService, IUserVmService
    {
        private readonly IIdentity _userIdentity;

        public SecureUserVmService(IUserVmRepository userVmRepo, IUnitOfWork unitOfWork, IIdentity userIdentity) : base(userVmRepo, unitOfWork)
        {
            this._userIdentity = userIdentity;
        }

        public override UserVm GetVmById(Guid id)
        {
            var vm = base.GetVmById(id);

            if(vm.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access denied for userVm with id={id.ToString()}");
            }

            return vm;
        }
    }
}
