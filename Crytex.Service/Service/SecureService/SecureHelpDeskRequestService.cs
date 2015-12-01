using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService.ISecureService;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace Crytex.Service.Service.SecureService
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

        public override HelpDeskRequest GetById(int id)
        {
            var request = base.GetById(id);

            if (request.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access for request with id={request.Id} is denied.");
            }

            return request;
        }

        protected override HelpDeskRequestComment GetCommentById(int id)
        {
            var comment = this.GetCommentById(id);

            if (comment.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access for commentq with id={comment.Id} is denied.");
            }

            return comment;
        }
    }
}
