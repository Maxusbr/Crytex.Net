using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Crytex.Model.Models
{
    public class HyperVHostResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public HostResourceType ResourceType { get; set; }
        public string Value { get; set; }
        public bool Valid { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid HyperVHostId { get; set; }

        [ForeignKey("HyperVHostId")]
        public HyperVHost HyperVHost { get; set; }
    }

    public enum HostResourceType
    {
        Hdd = 0
    }
}
