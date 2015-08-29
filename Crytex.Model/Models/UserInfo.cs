using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class UserInfo
    {
        [Key, ForeignKey("User")]
        public String UserId { get; set; }
        public decimal Balance { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
