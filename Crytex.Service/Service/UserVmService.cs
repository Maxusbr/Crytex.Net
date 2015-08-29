﻿using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;

namespace Crytex.Service.Service
{
    public class UserVmService : IUserVmService
    {
        private IUserVmRepository _userVmRepo;
        private IUnitOfWork _unitOfWork;

        public UserVmService(IUserVmRepository userVmRepo, IUnitOfWork unitOfWork)
        {
            this._userVmRepo = userVmRepo;
            this._unitOfWork = unitOfWork;
        }

        public UserVm GetVmById(Guid id)
        {
            var vm = this._userVmRepo.GetById(id);
            if (vm == null)
            {
                throw new InvalidIdentifierException(string.Format("UserVm with id={0} doesnt exist.", id));
            }

            return vm;
        }


        public IPagedList<UserVm> GetPage(int pageNumber, int pageSize, string userId)
        {
            var page = new Page(pageNumber, pageSize);
            var list = this._userVmRepo.GetPage(page, x => x.UserId == userId, x => x.Id);
            return list;
        }
    }
}