using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class SystemCenterVirtualManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Synchronize { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }

        [InverseProperty("VirtualManager")]
        public ICollection<HyperVHost> HyperVHosts { get; set; }
    }
}
