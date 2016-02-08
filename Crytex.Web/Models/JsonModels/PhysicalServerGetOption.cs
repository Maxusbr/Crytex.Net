using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class PhysicalServerGetOption
    {
        public string Id { get; set; }
        public PhysicalServerType Type { get; set; }
    }

    /// <summary>
    /// Тип конфигурации физического сервера
    /// </summary>
    public enum PhysicalServerType
    {
        /// <summary>
        /// конфигурация готового физического сервера
        /// </summary>
        ReadyServer = 1,
        /// <summary>
        /// конфигурация физического сервера со списком доступных опций
        /// </summary>
        AviableServer = 2
    }
}