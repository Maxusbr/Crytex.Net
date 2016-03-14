using System;
using System.ComponentModel.DataAnnotations;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    public class GameViewModel
    {
        [Required]
        public Int32 Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public GameFamily? Family { get; set; }

        [Required]
        public string Version { get; set; }

        public string VersionCode { get; set; }
    }
}