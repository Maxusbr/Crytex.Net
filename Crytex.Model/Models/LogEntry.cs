using System;

namespace Crytex.Model.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class LogEntry : BaseEntity
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Level { get; set; }
        public string Source { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}