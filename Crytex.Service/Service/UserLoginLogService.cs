using System;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using PagedList;
using System.Linq.Expressions;
using Crytex.Service.Extension;

namespace Crytex.Service.Service
{
    class UserLoginLogService : IUserLoginLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserLoginLogEntryRepository _userLoginLogEntryRepository;

        public UserLoginLogService(IUserLoginLogEntryRepository logEntryRepo, IUnitOfWork unitOfWork)
        {
            this._userLoginLogEntryRepository = logEntryRepo;
            this._unitOfWork = unitOfWork;
        }

        public UserLoginLogEntry CreateLogEntryForNow(string userId, string ipAddress, bool withDataSaving)
        {
            var newUserEntry = new UserLoginLogEntry
            {
                IpAddress = ipAddress,
                LoginDate = DateTime.UtcNow,
                UserId = userId.ToString(),
                WithDataSaving = withDataSaving
            };

            this._userLoginLogEntryRepository.Add(newUserEntry);
            this._unitOfWork.Commit();

            return newUserEntry;
        }

        public IPagedList<UserLoginLogEntry> GetPage(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null, string userId = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);

            Expression<Func<UserLoginLogEntry, bool>> where = x => true;
            if (from != null)
            {
                where = where.And(x => x.LoginDate >= from);
            }
            if (to != null)
            {
                where = where.And(x => x.LoginDate <= to);
            }
            if (userId != null)
            {
                where = where.And(x => x.UserId == userId);
            }

            var page = this._userLoginLogEntryRepository.GetPage(pageInfo, where, b => b.LoginDate);

            return page;
        }
    }
}
