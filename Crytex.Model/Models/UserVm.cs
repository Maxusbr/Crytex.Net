using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Crytex.Model.Models.Biling;
using System.Collections.Generic;

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
        public int OperatingSystemId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public TypeVirtualization VirtualizationType { get; set; }
        public Guid? HyperVHostId { get; set; }
        public Guid? VmWareCenterId { get; set; }
        public string OperatingSystemPassword { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CurrentSnapshotId { get; set; }

        [ForeignKey("OperatingSystemId")]
        public OperatingSystem OperatingSystem { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("HyperVHostId")]
        public HyperVHost HyperVHost { get; set; }
        [ForeignKey("VmWareCenterId")]
        public VmWareVCenter VmWareCenter { get; set; }
        [ForeignKey("CurrentSnapshotId")]
        public SnapshotVm CurrentSnapshot { get; set; }

        public virtual SubscriptionVm SubscriptionVm { get; set; }

        [InverseProperty("Vm")]
        public ICollection<VmIpAddress> IpAdresses { get; set; }
        [InverseProperty("Vm")]
        public ICollection<SnapshotVm> Snapshots { get; set; }

        public VmHardwareConfig GetVmHardwareConfiguration()
        {
            var vmConfig = new VmHardwareConfig
            {
                Cpu = this.CoreCount,
                HardDriveSizeGB = this.HardDriveSize,
                RamMB = this.RamCount
            };

            return vmConfig;
        }
    }

    public class VmHardwareConfig
    {
        public int Cpu { get; set; }
        public int HardDriveSizeGB { get; set; }
        public int RamMB { get; set; }
    }

    public enum StatusVM
    {
        Enable = 0,
        Disable = 1,
        Error = 2,
        Creating = 3,
        Deleted = 4
    }
}
