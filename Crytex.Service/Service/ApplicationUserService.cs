using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using PagedList;

namespace Crytex.Service.Service
{
    public class ApplicationUserService : IApplicationUserService
    {
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
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
    }
}