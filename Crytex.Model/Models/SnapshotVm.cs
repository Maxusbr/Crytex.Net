using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class SnapshotVm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public Guid VmId { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }

        public bool Validation { get; set; }
    }
}
