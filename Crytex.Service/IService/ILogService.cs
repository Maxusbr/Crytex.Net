using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface ILogService
    {
        IPagedList<LogEntry> GetLogEntries(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, string sourceLog);
        IPagedList<LogEntry> GetLogEntries(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, Expression<Func<LogEntry, bool>> addLogExpression = null);
        LogEntry GetLogEntry(int id);
    }
}