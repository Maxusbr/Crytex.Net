using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class SnapshotVmViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public Guid VmId { get; set; }

        public bool Validation { get; set; }
    }
}