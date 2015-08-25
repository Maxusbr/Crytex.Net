using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class UserVm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public StatusVM Status { get; set; }
        public Int32 ServerTemplateId { get; set; }

        [ForeignKey("ServerTemplateId")]
        public virtual ServerTemplate ServerTemplate { get; set; }
    }


    public enum StatusVM
    {
        Enable,
        Disable,
        Error,
        Creating
    }
}
