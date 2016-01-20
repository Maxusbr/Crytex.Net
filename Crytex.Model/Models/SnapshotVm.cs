using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class SnapshotVm : GuidBaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public Guid VmId { get; set; }
        public SnapshotStatus Status { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }

        public bool Validation { get; set; }
    }

    public enum SnapshotStatus
    {
        Creating = 0,
        Active = 1,
        Deleted = 2
    }
}
