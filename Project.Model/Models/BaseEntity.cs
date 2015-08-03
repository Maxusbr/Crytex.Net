using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
   public  class BaseEntity
    {
        [Key]
        public Int32 Id { get; set; }
    }
}
