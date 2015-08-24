using System;
using Project.Model.Models;

namespace Project.Web.Models.ViewModels
{
    public class LogEntryViewModel
    {
        public LogEntryViewModel(LogEntry logEntry)
        {
            Id = logEntry.Id;
            UserId = logEntry.UserId;
            UserName = logEntry.User?.UserName;
            Date = logEntry.Date;
            Message = logEntry.Message;
            StackTrace = logEntry.StackTrace;
            Level = logEntry.Level;
            Source = logEntry.Source;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Level { get; set; }
        public string Source { get; set; }
    }
}