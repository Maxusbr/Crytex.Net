using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Core;
using Project.Model.Models;
using Project.Service.IService;
using Project.Web.Models.ViewModels;

namespace Project.Web.Controllers.Api
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
            return _logService.GetLogEntries(pageSize, pageIndex, dateFrom, dateTo, sourceLog).Select(x => new LogEntryViewModel(x)).ToList();
        }

        // GET api/<controller>/5
        public LogEntryViewModel Get(int id)
        {
            var logEntry = _logService.GetLogEntry(id);
            return new LogEntryViewModel(logEntry);
        }
    }
}