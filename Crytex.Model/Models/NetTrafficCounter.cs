using Crytex.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class NetTrafficCounter : BaseEntity
    {
        public Guid MachineId { get; set; }
        public long ReceivedBytes { get; set; }
        public long TransmittedBytes { get; set; }
        public CountingPeriodType PeriodType { get; set; }
        public DateTime CountingPeriodStartDate { get; set; }

        [ForeignKey("MachineId")]
        public UserVm Vm { get; set; }
    }
}
