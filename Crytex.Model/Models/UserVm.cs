﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class UserVm
    {
        [Key]
        public Guid Id { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public StatusVM Status { get; set; }
        public Int32 ServerTemplateId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public TypeVirtualization VurtualizationType { get; set; }
        public Guid? HyperVHostId { get; set; }
        public Guid? VmWareCenterId { get; set; }
        public string OperatingSystemPassword { get; set; }

        [ForeignKey("ServerTemplateId")]
        public ServerTemplate ServerTemplate { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("HyperVHostId")]
        public HyperVHost HyperVHost { get; set; }
        [ForeignKey("VmWareCenterId")]
        public VmWareVCenter VmWareCenter { get; set; }
    }


    public enum StatusVM
    {
        Enable,
        Disable,
        Error,
        Creating
    }
}
