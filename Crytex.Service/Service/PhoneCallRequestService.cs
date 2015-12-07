using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Exceptions;
using PagedList;

namespace Crytex.Service.Service
{
    public class PhoneCallRequestService : IPhoneCallRequestService
    {
        private readonly IPhoneCallRequestRepository _phoneCallRequestRepo;
        private readonly IUnitOfWork _uniOfWork;

        public PhoneCallRequestService(IPhoneCallRequestRepository requestRepo, IUnitOfWork unitOfWork)
        {
            this._phoneCallRequestRepo = requestRepo;
            this._uniOfWork = unitOfWork;
        }

        public PhoneCallRequest Create(PhoneCallRequest request)
        {
            request.CreationDate = DateTime.UtcNow;
            request.IsRead = false;

            this._phoneCallRequestRepo.Add(request);
            this._uniOfWork.Commit();

            return request;
        }

        public PhoneCallRequest GetById(int id)
        {
            var request = this.GetByIdOrThrowException(id);

            return request;
        }

        public IPagedList<PhoneCallRequest> GetPage(int pageNumber, int pageSize)
        {
            var page = new PageInfo(pageNumber, pageSize);
            var pagedList = this._phoneCallRequestRepo.GetPage(page, x => true, x => x.CreationDate);

            return pagedList;
        }

        public void Update(int id, PhoneCallRequest request)
        {
            var requestToUpdate = this.GetByIdOrThrowException(id);
            requestToUpdate.IsRead = request.IsRead;

            this._phoneCallRequestRepo.Update(requestToUpdate);
            this._uniOfWork.Commit();
        }

        private PhoneCallRequest GetByIdOrThrowException(int id)
        {
            var request = this._phoneCallRequestRepo.GetById(id);

            if (request == null)
            {
                throw new InvalidIdentifierException($"Request with id={id} doesnt exist");
            }

            return request;
        }
    }
}
