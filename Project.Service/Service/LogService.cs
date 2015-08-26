using System;
using System.Collections.Generic;
using System.Linq;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using Project.Service.IService;

namespace Project.Service.Service
{
    public class LogService : ILogService
    {
        private ILogRepository _logRepository { get; }

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public List<LogEntry> GetLogEntries()
        {
            return _logRepository.GetAll().ToList();
        }

        public List<LogEntry> GetLogEntries(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, string sourceLog)
        {
            var logEntries = _logRepository.GetPage(new Page(pageIndex, pageSize),
                    x => (string.IsNullOrEmpty(sourceLog) || x.Source == sourceLog) && 
                         (!dateFrom.HasValue || !dateTo.HasValue || x.Date > dateFrom.Value && x.Date < dateTo.Value),
                    x => x.Id).ToList();

            return logEntries;
        }

        public LogEntry GetLogEntry(int id)
        {
            return _logRepository.GetById(id);
        }
    }
}