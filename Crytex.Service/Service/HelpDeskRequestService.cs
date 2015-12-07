using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Exceptions;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.Service
{
    public class HelpDeskRequestService : IHelpDeskRequestService
    {
        private readonly IHelpDeskRequestRepository _requestRepository;
        private readonly IHelpDeskRequestCommentRepository _requestCommentRepository;
        private readonly IFileDescriptorRepository _fileDescriptorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HelpDeskRequestService(IHelpDeskRequestRepository requestRepo,
            IHelpDeskRequestCommentRepository requestCommentRepo, IFileDescriptorRepository fileDescriptorRepository, IUnitOfWork unitOfWork)
        {
            this._requestRepository = requestRepo;
            this._requestCommentRepository = requestCommentRepo;
            this._unitOfWork = unitOfWork;
            this._fileDescriptorRepository = fileDescriptorRepository;
        }

        public HelpDeskRequest CreateNew(HelpDeskRequest request)
        {
            request.Read = false;
            request.Status = RequestStatus.New;
            request.CreationDate = DateTime.UtcNow;

            var requestFileIds = request.FileDescriptors.Select(x => x.Id).ToArray();
            var requestFiles = this._fileDescriptorRepository.GetMany(x => requestFileIds.Contains(x.Id));
            request.FileDescriptors = requestFiles;

            this._requestRepository.Add(request);
            this._unitOfWork.Commit();

            return request;
        }


        public virtual void Update(HelpDeskRequest request)
        {
            var requestToUpdate = this.GetById(request.Id);

            if (requestToUpdate == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequest width Id={0} doesn't exists", request.Id));
            }

            requestToUpdate.Details = request.Details;
            requestToUpdate.Summary = request.Summary;
            requestToUpdate.Status = request.Status;
            requestToUpdate.Read = request.Read;

            this._requestRepository.Update(requestToUpdate);
            this._unitOfWork.Commit();
        }

        // Use this method to get HelpDeskRequest intstance by id intead of using HelpDeskRequestRepository directly!!!
        public virtual HelpDeskRequest GetById(int id)
        {
            var request = this._requestRepository.Get(h => h.Id == id, r => r.User, r => r.FileDescriptors);

            if (request == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequest width Id={0} doesn't exists", id));
            }

            return request;
        }

      
        public virtual void DeleteById(int id)
        {
            
            var request = this.GetById(id);

        
            this._requestRepository.Delete(request);
            this._unitOfWork.Commit();
        }

        public IPagedList<HelpDeskRequest> GetPage(int pageNumber, int pageSize, HelpDeskRequestFilter filter = HelpDeskRequestFilter.All)
        {
            IPagedList<HelpDeskRequest> page = new PagedList<HelpDeskRequest>(Enumerable.Empty<HelpDeskRequest>().AsQueryable(), 1, 1);
            if (filter == HelpDeskRequestFilter.All) { 
             
                page = this._requestRepository.GetPage(new PageInfo(pageNumber, pageSize), (x => true), (x => x.Read), (x => x.CreationDate));
            }
            else if (filter == HelpDeskRequestFilter.Read)
            {
               
                page = this._requestRepository.GetPage(new PageInfo(pageNumber, pageSize), (x => x.Read), (x => x.CreationDate));
            }
            else if (filter == HelpDeskRequestFilter.Unread)
            {
                page = this._requestRepository.GetPage(new PageInfo(pageNumber, pageSize), (x => !x.Read), (x => x.CreationDate));
            }

            return page;
        }


     
        public virtual IEnumerable<HelpDeskRequestComment> GetCommentsByRequestId(int id)
        {
           
            var request = this.GetById(id);

            var comments = _requestCommentRepository.GetMany(c=>c.RequestId == id, i=>i.User);

            return comments;
        }

   
        public virtual IPagedList<HelpDeskRequestComment> GetPageCommentsByRequestId(int id, int pageNumber, int pageSize)
        {
           
            var request = this.GetById(id);

            var comments = _requestCommentRepository.GetPage(new PageInfo(pageNumber, pageSize), (x => x.RequestId == request.Id), (x => x.CreationDate));

         
            return comments;
        }


        public HelpDeskRequestComment CreateComment(int requestId, string comment, string userId, bool isRead = false)
        {
          
            var request = this.GetById(requestId);

        
            var newComment = new HelpDeskRequestComment
            {
                Comment = comment,
                UserId = userId,
                RequestId = requestId,
                CreationDate = DateTime.UtcNow,
            };

            request.Read = isRead;

            this._requestCommentRepository.Add(newComment);
            this._unitOfWork.Commit();

            return newComment;
        }

       
        public virtual void DeleteCommentById(int id)
        {
         
            var comment = this.GetCommentById(id);

        
            this._requestCommentRepository.Delete(comment);
            this._unitOfWork.Commit();
        }


        public virtual void UpdateComment(int commentId, string comment)
        {
          
            var commentToUpdate = this.GetCommentById(commentId);

       
            commentToUpdate.Comment = comment;

            this._requestCommentRepository.Update(commentToUpdate);
            this._unitOfWork.Commit();
        }

        protected virtual HelpDeskRequestComment GetCommentById(int id)
        {
            var comment = this._requestCommentRepository.GetById(id);

            if (comment == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequestComment width Id={0} doesn't exists", id));
            }

            return comment;
        }
    }
}
