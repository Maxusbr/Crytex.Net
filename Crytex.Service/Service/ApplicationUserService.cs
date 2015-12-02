using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
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
        private IUserVmRepository _userVmRepository;
        private IUnitOfWork _unitOfWork;
        
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository, IUnitOfWork unitOfWork, IUserVmRepository userVmRepository)
        {
            _applicationUserRepository = applicationUserRepository;
            _userVmRepository = userVmRepository;
            _unitOfWork = unitOfWork;
        }

        IApplicationUserRepository _applicationUserRepository { get; }

        public List<ApplicationUser> GetAll()
        {
            return _applicationUserRepository.GetAll();
        }

        public IPagedList<ApplicationUser> GetPage(int pageNumber, int pageSize, ApplicationUserSearchParams searchParams = null)
        {
            //return _applicationUserRepository.GetPage(new Page(pageNumber, pageSize),
            //        x => (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(email)) 
            //        || (!string.IsNullOrEmpty(userName) && x.UserName.Contains(userName)) 
            //        || (!string.IsNullOrEmpty(email) && x.Email.Contains(email)),
            //        x => x.Id);

            var page = new Page(pageNumber, pageSize);

            Expression<Func<ApplicationUser, bool>> where = x => true;

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

        public List<ApplicationUser> Search(string searchParam)
        {
            return _applicationUserRepository.GetMany(x => x.UserName.Contains(searchParam) || x.Email.Contains(searchParam));
        }

        public void DeleteUser(ApplicationUser user)
        {
            _userVmRepository.Delete(v=>v.UserId == user.Id);
            _applicationUserRepository.Delete(u=>u.Id == user.Id);
            _unitOfWork.Commit();
        }
    }
}