using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class GameServer : BaseEntity
    {
        public ServerPaymentType PaymentType { get; set; }
        public Guid VmId { get; set; }
        public int SlotCount { get; set; }
        public int GameServerConfigurationId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }
        [ForeignKey("GameServerConfigurationId")]
        public GameServerConfiguration GameServerConfiguration { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }

    public enum ServerPaymentType
    {
        Slot = 0,
        Configuration = 1
    }
}
