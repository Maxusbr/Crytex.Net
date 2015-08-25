using System;
using System.Collections.Generic;
using Project.Model.Models;

namespace Project.Service.IService
{
    public interface ILogService
    {
        List<LogEntry> GetLogEntries(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, string sourceLog);

        LogEntry GetLogEntry(int id);
    }
}