using Crytex.Model.Models;
using PagedList;
using System;

namespace Crytex.Service.IService
{
    public interface IUserLoginLogService
    {
        UserLoginLogEntry CreateLogEntryForNow(string userId, string ipAddress, bool withDataSaving);
        IPagedList<UserLoginLogEntry> GetPage(int pageNumber, int pageSize, DateTime? from, DateTime? to, string userId);
    }
}
