using System;
using Crytex.Model.Models;

namespace Crytex.Web.Models.ViewModels
{
    public class LogEntryViewModel
    {
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