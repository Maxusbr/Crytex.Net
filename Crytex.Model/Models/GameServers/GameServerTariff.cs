using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.GameServers
{
    public class GameServerTariff : BaseEntity
    {
        public int GameId { get; set; }
        // Стоимость слота
        public decimal Slot { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; }
    }
}
