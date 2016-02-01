using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Service.IService;
using System.Security.Principal;
using Crytex.Model.Models.WebHostingModels;
using System;
using Microsoft.AspNet.Identity;
using Crytex.Model.Exceptions;

namespace Crytex.Service.Service.SecureService
{
    class SecuredHostedWebApplicationService : HostedWebApplicationService, IHostedWebApplicationService
    {
        private readonly IIdentity _userIdentity;

        public SecuredHostedWebApplicationService(IHostedWebApplicationRepository hostedWebApplicationRepository, ITaskV2Service taskService,
            IUnitOfWork unitOfWork, IIdentity userIdentity) : base(hostedWebApplicationRepository, taskService, unitOfWork)
        {
            this._userIdentity = userIdentity;
        }

        public override HostedWebApplication GetHostedWebApplicationById(Guid id)
        {
            var app = base.GetHostedWebApplicationById(id);

            if(app.WebHosting.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access for HostedWebApplication with id={id.ToString()} is denied");
            }

            return app;
        }
    }
}
