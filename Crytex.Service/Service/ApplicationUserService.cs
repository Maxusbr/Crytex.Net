﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Microsoft.Practices.Unity;
using PagedList;

namespace Crytex.Service.Service
{
    public class ApplicationUserService : IApplicationUserService
    {
        private IUnitOfWork _unitOfWork;
        
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository, IUnitOfWork unitOfWork)
        {
            _applicationUserRepository = applicationUserRepository;
            _unitOfWork = unitOfWork;
        }

        IApplicationUserRepository _applicationUserRepository { get; }

        public List<ApplicationUser> GetAll()
        {
            return _applicationUserRepository.GetAll();
        }

        public IPagedList<ApplicationUser> GetPage(int pageNumber, int pageSize, ApplicationUserSearchParams searchParams = null)
        {
            var page = new PageInfo(pageNumber, pageSize);

            Expression<Func<ApplicationUser, bool>> where = x => x.Deleted == false;

            if (searchParams != null)
            {
                if (searchParams.Name != null) where = where.And(x => x.Name == searchParams.Name);
                
                if (searchParams.Lastname != null) where = where.And(x => x.Lastname == searchParams.Lastname);

                if (searchParams.Patronymic != null) where = where.And(x => x.Patronymic == searchParams.Patronymic);

                if (searchParams.Email != null) where = where.And(x => x.Email == searchParams.Email);

                if (searchParams.UserName != null) where = where.And(x => x.UserName == searchParams.UserName);

                if (searchParams.TypeOfUser != null) where = where.And(x => x.UserType == searchParams.TypeOfUser);

                if (searchParams.RegisterDateFrom != null) where = where.And(x => x.RegisterDate >= searchParams.RegisterDateFrom);

                if (searchParams.RegisterDateTo != null) where = where.And(x => x.RegisterDate <= searchParams.RegisterDateTo);                
            }

            var userList = this._applicationUserRepository.GetPage(page, where, x => x.RegisterDate);
            return userList;
        }

        public ApplicationUser GetUserById(string id)
        {
            return _applicationUserRepository.GetById(id);
        }

        public void UpdateStateUser(UpdateUserState data)
        {
            var user = this._applicationUserRepository.GetById(data.UserId);

            if (user == null)
            {
                throw new InvalidIdentifierException(string.Format("User width Id={0} doesn't exists", data.UserId));
            }

            user.IsBlocked = data.Block;
            _unitOfWork.Commit();
        }

        public List<ApplicationUser> Search(string searchParam)
        {
            return _applicationUserRepository.GetMany(x => x.UserName.Contains(searchParam) || x.Email.Contains(searchParam));
        }

        public void DeleteUser(string id)
        {
            var user = this._applicationUserRepository.GetById(id);

            if (user == null)
            {
                throw new InvalidIdentifierException(string.Format("User width Id={0} doesn't exists", id));
            }
            user.Deleted = true;
            _applicationUserRepository.Update(user);
            _unitOfWork.Commit();
        }
    }
}