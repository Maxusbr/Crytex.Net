using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class GameServerConfiguration : BaseEntity
    {
        public int ServerTemplateId { get; set; }

        [ForeignKey("ServerTemplateId")]
        public ServerTemplate ServerTemplate { get; set; }

        public decimal Processor1 { get; set; }
        public decimal RAM512 { get; set; }
        public decimal Slot { get; set; }
    }
}
