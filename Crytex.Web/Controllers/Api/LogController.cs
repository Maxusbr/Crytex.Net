using System;
using System.Collections.Generic;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.ViewModels;

namespace Crytex.Web.Controllers.Api
{
    public class LogController : ApiController
    {
        private ILogService _logService { get; }

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        // GET api/<controller>
        public List<LogEntryViewModel> Get(int pageSize = 20, int pageIndex = 1, DateTime? dateFrom = null, DateTime? dateTo = null, string sourceLog = null)
        {
            var logEntries = _logService.GetLogEntries(pageSize, pageIndex, dateFrom, dateTo, sourceLog);
            var model = AutoMapper.Mapper.Map<List<LogEntry>, List<LogEntryViewModel>>(logEntries);
            return model;
        }

        // GET api/<controller>/5
        public LogEntryViewModel Get(int id)
        {
            var logEntry = _logService.GetLogEntry(id);
            var model = AutoMapper.Mapper.Map<LogEntry, LogEntryViewModel>(logEntry);
            return model;
        }
    }
}