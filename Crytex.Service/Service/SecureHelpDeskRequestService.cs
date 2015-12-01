using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Service
{
    public class SecureHelpDeskRequestService : HelpDeskRequestService, ISecureHelpDeskRequestService
    {
        private readonly IIdentity _userIdentity;

        public SecureHelpDeskRequestService(IHelpDeskRequestRepository requestRepo,
            IHelpDeskRequestCommentRepository requestCommentRepo, IFileDescriptorRepository fileDescriptorRepository, IUnitOfWork unitOfWork,
            IIdentity userIdentity) :
                base(requestRepo, requestCommentRepo, fileDescriptorRepository, unitOfWork)
        {
            this._userIdentity = userIdentity;
        }

        public override HelpDeskRequest GeById(int id)
        {
            var request = base.GeById(id);

            ThrowSecurityExceptionIfNeeded(request);

            return request;
        }

        public override void Update(HelpDeskRequest request)
        {
            var requestToUpdate = this.GeById(request.Id);

            ThrowSecurityExceptionIfNeeded(requestToUpdate);

            base.Update(requestToUpdate);
        }

        public override void DeleteById(int id)
        {
            var request = base.GeById(id);

            ThrowSecurityExceptionIfNeeded(request);

            base.DeleteById(id);
        }

        private void ThrowSecurityExceptionIfNeeded(HelpDeskRequest request)
        {
            if (request.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access for request with id={request.Id} is denied.");
            }
        }
    }
}
