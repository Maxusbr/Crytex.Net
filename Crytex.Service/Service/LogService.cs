using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Core;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using PagedList;

namespace Crytex.Service.Service
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
            return _logRepository.GetAll();
        }

        public IPagedList<LogEntry> GetLogEntries(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, string sourceLog)
        {
            var minDate = dateFrom ?? DateTime.MinValue;
            var maxDate = dateTo ?? DateTime.MaxValue;

            var logEntries = _logRepository.GetPage(new Page(pageIndex, pageSize),
                    x => (string.IsNullOrEmpty(sourceLog) || x.Source == sourceLog) && 
                         (x.Date >= minDate && x.Date <= maxDate),
                    x => x.Id);

            return logEntries;
        }

        public IPagedList<LogEntry> GetLogEntries(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, Expression<Func<LogEntry, bool>> addLogExpression = null)
        {
            var minDate = dateFrom ?? DateTime.MinValue;
            var maxDate = dateTo ?? DateTime.MaxValue;

            Expression<Func<LogEntry, bool>> where = x => (x.Date >= minDate && x.Date <= maxDate);

            if (addLogExpression != null)
            {
                where.And(addLogExpression);
            }

            var logEntries = _logRepository.GetPage(new Page(pageIndex, pageSize), where, x => x.Id);

            return logEntries;
        }
        
        public LogEntry GetLogEntry(int id)
        {
            return _logRepository.GetById(id);
        }
    }
}