using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    /// <summary>
    /// Конфигурация физического сервера
    /// </summary>
    public class PhysicalServerViewModel
    {
        public string Id { get; set; }
        [Required]
        public string ProcessorName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// Конфигурация опций по умолчанию
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// Опции сервера
        /// </summary>
        public ICollection<PhysicalServerOptionViewModel> Options { get; set; }
    }
}