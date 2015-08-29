using Crytex.Model.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class CreateVmTaskViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int? Cpu { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int? Ram { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int? Hdd { get; set; }
        [Required]
        public int? ServerTemplateId { get; set; }
        public StatusTask StatusTask { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImagePath { get; set; }
    }
}
