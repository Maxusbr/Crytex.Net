using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerConfigurationView
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public int ServerTemplateId { get; set; }
        /// <summary>
        /// Стоимость процессора
        /// </summary>
        public decimal Processor1 { get; set; }
        /// <summary>
        /// Стоимость памяти
        /// </summary>
        public decimal RAM512 { get; set; }
        /// <summary>
        /// Стоимость слота
        /// </summary>
        public decimal Slot { get; set; }
    }
}