using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Model.Models
{
   public  class BaseEntity
    {
        [Key]
        public Int32 Id { get; set; }
    }
}
