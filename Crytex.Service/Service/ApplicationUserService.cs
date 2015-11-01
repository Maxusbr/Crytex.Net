using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
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
            return _applicationUserRepository.GetAll().ToList();
        }

        public IPagedList<ApplicationUser> GetPage(int pageSize, int pageIndex, string userName, string email)
        {
            return _applicationUserRepository.GetPage(new Page(pageIndex, pageSize),
                    x => (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(email)) 
                    || (!string.IsNullOrEmpty(userName) && x.UserName.Contains(userName)) 
                    || (!string.IsNullOrEmpty(email) && x.Email.Contains(email)),
                    x => x.Id);
        }

        public ApplicationUser GetUserById(string id)
        {
            return _applicationUserRepository.GetById(id);
        }

        public void DeleteUser(ApplicationUser user)
        {
            _userVmRepository.Delete(v=>v.UserId == user.Id);
            _applicationUserRepository.Delete(u=>u.Id == user.Id);
            _unitOfWork.Commit();
        }
    }
}