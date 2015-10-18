using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Core;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Crytex.Web.Models.ViewModels;
using PagedList;

namespace Crytex.Web.Areas.Admin
{
    public class AdminLogController : AdminCrytexController
    {
        private ILogService _logService { get; }

        public AdminLogController(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Получение списка Логов
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="sourceLog"></param>
        /// <returns></returns>
        // GET api/<controller>
        [ResponseType(typeof(PageModel<LogEntryViewModel>))]
        public IHttpActionResult Get(int pageSize = 20, int pageNumber = 1, DateTime? dateFrom = null, DateTime? dateTo = null, SourceLog? sourceLog = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            IPagedList<LogEntry> logEntries = new PagedList<LogEntry>(new List<LogEntry>(),1,1);

            if (sourceLog != null)
            {
                logEntries = _logService.GetLogEntries(pageSize, pageNumber, dateFrom, dateTo,
                    sourceLog.Value.ToString("G"));
            }
            else
            {
                logEntries = _logService.GetLogEntries(pageSize, pageNumber, dateFrom, dateTo);
            }

            var model = AutoMapper.Mapper.Map<PageModel<LogEntryViewModel>>(logEntries);

            return Ok(model);
        }

        /// <summary>
        /// Получение лога по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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