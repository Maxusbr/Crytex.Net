using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Model.Models
{
    public class Message
    {
        [Key]
        public Int32 Id { get; set; }
        
        public String Body { get; set; }
    }
}
