using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    /// <summary>
    /// Опция физического сервера
    /// </summary>
    public class PhysicalServerOptionViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public PhysicalServerOptionType Type { get; set; }

        /// <summary>
        /// Опция по умолчанию?
        /// </summary>
        public bool IsDefault { get; set; }
    }
}