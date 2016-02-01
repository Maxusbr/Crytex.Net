using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class WebHostingTariffViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int StorageSizeGB { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int DomainCount { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int FtpAccountCount { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int DatabaseCount { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}