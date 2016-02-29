using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerTariffView
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        /// <summary>
        /// Стоимость слота
        /// </summary>
        public decimal Slot { get; set; }
    }
}