using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class UserLoginLogEntry : BaseEntity
    {
        public string UserId { get; set; }
        public DateTime LoginDate { get; set; }
        public string IpAddress { get; set; }
        public bool WithDataSaving { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
