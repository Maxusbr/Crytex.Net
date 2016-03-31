using System;

namespace Crytex.Model.Models
{
    public class Location : GuidBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Disabled { get; set; }
    }
}
