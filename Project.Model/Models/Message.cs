using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Model.Models
{
    public class Message
    {
        [Key]
        public Int32 Id { get; set; }
        
        public String Body { get; set; }
    }
}
