using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class VmBackup : GuidBaseEntity
    {
        public Guid VmId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public VmBackupStatus Status { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }
        
    }
    public enum VmBackupStatus
    {
        Creting = 0,
        Active = 1,
        Deleted = 2
    }
}
