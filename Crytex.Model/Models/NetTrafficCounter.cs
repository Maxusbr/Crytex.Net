using Crytex.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class NetTrafficCounter : BaseEntity
    {
        public Guid MachineId { get; set; }
        public long ReceiveKiloBytes { get; set; }
        public long TransmittedKiloBytes { get; set; }
        public CountingPeriodType PeriodType { get; set; }
        public DateTime CountingPeriodStartDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        [ForeignKey("MachineId")]
        public UserVm Vm { get; set; }
    }
}
