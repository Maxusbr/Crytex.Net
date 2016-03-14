using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Crytex.Model.Enums;

namespace Crytex.Model.Models.GameServers
{
    public class Game : BaseEntity
    {
        public string Name { get; set; }
        public GameFamily Family { get; set; }
        public string Version { get; set; }
        public string VersionCode { get; set; }
        public int ImageFileDescriptorId { get; set; }

        public ICollection<GameHost> GameHosts { get; set; } 
        [ForeignKey("ImageFileDescriptorId")]
        public FileDescriptor ImageFileDescriptor { get; set; }
    }

}
