using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class UserVm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public StatusVM Status { get; set; }
        public Int32 ServerTemplateId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        [ForeignKey("ServerTemplateId")]
        public ServerTemplate ServerTemplate { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }


    public enum StatusVM
    {
        Enable,
        Disable,
        Error,
        Creating
    }
}
