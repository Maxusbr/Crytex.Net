using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface ILogService
    {
        List<LogEntry> GetLogEntries(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, string sourceLog);

        LogEntry GetLogEntry(int id);
    }
}