using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class VmBackup
    {
        [Key]
        public Guid Id { get; set; }
        public Guid VmId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }
    }
}
