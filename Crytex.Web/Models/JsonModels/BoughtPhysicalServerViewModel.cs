using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Crytex.Model.Enums;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class BoughtPhysicalServerViewModel
    {
        public string Id { get; set; }
        [Required]
        public string PhysicalServerId { get; set; }
        public string UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? DateEnd { get; set; }
        [Required]
        public int CountMonth { get; set; }
        public decimal DiscountPrice { get; set; }
        public BoughtPhysicalServerStatus Status { get; set; }
        /// <summary>
        /// Конфигурация сервера
        /// </summary>
        public string Config { get; set; }
        public bool AutoProlongation { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("PhysicalServerId")]
        public PhysicalServerViewModel Server { get; set; }

        public ICollection<PhysicalServerOptionViewModel> Options { get; set; }
    }
}