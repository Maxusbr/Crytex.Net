using System;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerTariffView
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        /// <summary>
        /// Стоимость слота
        /// </summary>
        /// 
        /// 
        public decimal Slot { get; set; }

        public string Name { get; set; }

        public int Performance { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Disabled { get; set; }
    }
}