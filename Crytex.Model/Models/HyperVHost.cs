using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class HyperVHost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Host { get; set; }
        public int CoreNumber { get; set; }
        public int RamSize { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Valid { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid SystemCenterVirtualManagerId { get; set; }
        public bool Deleted { get; set; }

        [ForeignKey("SystemCenterVirtualManagerId")]
        public SystemCenterVirtualManager VirtualManager { get; set; }
        [InverseProperty("HyperVHost")]
        public virtual ICollection<HyperVHostResource> Resources { get; set; }
    }
}
