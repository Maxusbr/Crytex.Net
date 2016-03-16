using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.GameServers
{
    public class GameServerTariff : BaseEntity
    {
        public string Name { get; set; }
        public int GameId { get; set; }
        public int Performance { get; set; }
        public bool Disabled { get; set; }
        // Стоимость слота
        public decimal Slot { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; }
    }
}
