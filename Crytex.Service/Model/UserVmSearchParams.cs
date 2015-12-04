using System;
using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class UserVmSearchParams
    {
        public string UserId { get; set; }

        [EnumDataType(typeof(TypeVirtualization))]
        public TypeVirtualization? Virtualization { get; set; }

        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
    }
}
