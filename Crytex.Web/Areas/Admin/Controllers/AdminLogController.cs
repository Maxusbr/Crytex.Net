using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Crytex.Web.Models.ViewModels;

namespace Crytex.Web.Areas.Admin
{
    public class AdminLogController : AdminCrytexController
    {
        private ILogService _logService { get; }

        public AdminLogController(ILogService logService)
        {
            _logService = logService;
        }

        // GET api/<controller>
        [ResponseType(typeof(List<LogEntryViewModel>))]
        public IHttpActionResult Get(int pageSize = 20, int pageNumber = 1, DateTime? dateFrom = null, DateTime? dateTo = null, string sourceLog = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }
            var logEntries = _logService.GetLogEntries(pageSize, pageNumber, dateFrom, dateTo, sourceLog);
            var model = AutoMapper.Mapper.Map<List<LogEntry>, List<LogEntryViewModel>>(logEntries);

            return Ok(model);
        }

        // GET api/<controller>/5
        [ResponseType(typeof(LogEntryViewModel))]
        public IHttpActionResult Get(int id)
        {
            var logEntry = _logService.GetLogEntry(id);
            var model = AutoMapper.Mapper.Map<LogEntry, LogEntryViewModel>(logEntry);
            return Ok(model);
        }


    }
}