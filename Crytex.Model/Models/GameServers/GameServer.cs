using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Crytex.Model.Models.Biling;

namespace Crytex.Model.Models.GameServers
{
    public class GameServer : GuidBaseEntity
    {
        public string Name { get; set; }
        public int SlotCount { get; set; }
        public int GameHostId { get; set; }
        public int GameServerTariffId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateExpire { get; set; }
        public GameServerStatus Status { get; set; }
        public GameServerState ServerState { get; set; }
        public bool AutoProlongation { get; set; }
        public int FirstPortInRange { get; set; }
        public int PortRangeSize { get; set; }
        public string Password { get; set; }

        [ForeignKey("GameHostId")]
        public GameHost GameHost { get; set; }
        [ForeignKey("GameServerTariffId")]
        public GameServerTariff GameServerTariff { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public virtual ICollection<BillingTransaction> BillingTransaction { get; set; }
    }
    public enum GameServerStatus
    {
        WaitForPayment = 0,
        Active = 1,
        Deleted = 2
    }

    public enum GameServerState
    {
        Creating = 0,
        Enable = 1,
        Disable = 2
    }
}