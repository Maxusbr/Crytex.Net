﻿using Project.Model.Models;
using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.IRepository;
using Project.Data.Infrastructure;
using Project.Model.Exceptions;
using PagedList;

namespace Project.Service.Service
{
    public class HelpDeskRequestService : IHelpDeskRequestService
    {
        private readonly IHelpDeskRequestRepository _requestRepository;
        private readonly IHelpDeskRequestCommentRepository _requestCommentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HelpDeskRequestService(IHelpDeskRequestRepository requestRepo,
            IHelpDeskRequestCommentRepository requestCommentRepo, IUnitOfWork unitOfWork)
        {
            this._requestRepository = requestRepo;
            this._requestCommentRepository = requestCommentRepo;
            this._unitOfWork = unitOfWork;
        }

        public HelpDeskRequest CreateNew(string summary, string details, string userId)
        {
            var newRequest = new HelpDeskRequest
            {
                Summary = summary,
                Details = details,
                UserId = userId,
                Status = RequestStatus.New,
                CreationDate = DateTime.UtcNow
            };

            this._requestRepository.Add(newRequest);
            this._unitOfWork.Commit();

            return newRequest;
        }


        public void Update(HelpDeskRequest request)
        {
            var requestToUpdate = this._requestRepository.GetById(request.Id);

            if (requestToUpdate == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequest width Id={0} doesn't exists", request.Id));
            }

            requestToUpdate.Details = request.Details;
            requestToUpdate.Summary = request.Summary;
            requestToUpdate.Status = request.Status;
            
            this._requestRepository.Update(requestToUpdate);
            this._unitOfWork.Commit();
        }


        public HelpDeskRequest GeById(int id)
        {
            var request = this._requestRepository.GetById(id);

            if (request == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequest width Id={0} doesn't exists", id));
            }

            return request;
        }

        public void DeleteById(int id)
        {
            var request = this._requestRepository.GetById(id);

            if (request == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequest width Id={0} doesn't exists", id));
            }

            this._requestRepository.Delete(request);
            this._unitOfWork.Commit();
        }

        public IPagedList<HelpDeskRequest> GetPage(int pageNumber, int pageSize)
        {
            var page = this._requestRepository.GetPage(new Page(pageNumber, pageSize), (x => true), (x => x.CreationDate));

            return page;
        }


        public IEnumerable<HelpDeskRequestComment> GetCommentsByRequestId(int id)
        {
            var request = this._requestRepository.GetById(id);

            if (request == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequest width Id={0} doesn't exists", id));
            }

            var comments = request.Comments;

            return comments;
        }


        public HelpDeskRequestComment CreateComment(int requestId, string comment, string userId)
        {
            var request = this._requestRepository.GetById(requestId);

            if (request == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequest width Id={0} doesn't exists", requestId));
            }

            var newComment = new HelpDeskRequestComment
            {
                Comment = comment,
                UserId = userId,
                RequestId = requestId,
                CreationDate = DateTime.UtcNow
            };
            this._requestCommentRepository.Add(newComment);
            this._unitOfWork.Commit();

            return newComment;
        }

        public void DeleteCommentById(int id)
        {
            var comment = this._requestCommentRepository.GetById(id);

            if (comment == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequestComment width Id={0} doesn't exists", id));
            }

            this._requestCommentRepository.Delete(comment);
            this._unitOfWork.Commit();
        }

        public void UpdateComment(int commentId, string comment)
        {
            var commentToUpdate = this._requestCommentRepository.GetById(commentId);

            if (commentToUpdate == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequestComment width Id={0} doesn't exists", commentId));
            }

            commentToUpdate.Comment = comment;

            this._requestCommentRepository.Update(commentToUpdate);
            this._unitOfWork.Commit();
        }
    }
}
