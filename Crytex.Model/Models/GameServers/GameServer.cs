using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Crytex.Model.Models.Biling;

namespace Crytex.Model.Models.GameServers
{
    public class GameServer : GuidBaseEntity
    {
        public string Name { get; set; }
        public Guid VmId { get; set; }
        public int SlotCount { get; set; }
        public int GameServerTariffId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateExpire { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }

        [ForeignKey("GameServerTariffId")]

        public GameServerTariff GameServerTariff { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public virtual ICollection<BillingTransaction> BillingTransaction { get; set; }
    }
}