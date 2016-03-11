using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Service.IService;
using System.Security.Principal;
using Crytex.Model.Models;
using System;
using Crytex.Model.Exceptions;
using Microsoft.AspNet.Identity;

namespace Crytex.Service.Service.SecureService
{
    public class SecureTaskV2Service : TaskV2Service, ITaskV2Service
    {
        private readonly IIdentity _userIdentity;

        public SecureTaskV2Service(ITaskV2Repository taskV2Repo, IUserVmService userVmService, IVmBackupService vmBackupService, 
            IUnitOfWork unitOfWork, IIdentity userIdentity) 
            : base(taskV2Repo, userVmService, unitOfWork, vmBackupService)
        {
            this._userIdentity = userIdentity;
        }

        public override TaskV2 GetTaskById(Guid id)
        {
            var task = base.GetTaskById(id);

            if(task.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access denied for task with id={id.ToString()}");
            }

            return task;
        }
    }
}
