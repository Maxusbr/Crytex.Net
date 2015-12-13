using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class GameServerConfiguration : BaseEntity
    {
        public int? ServerTemplateId { get; set; }

        [ForeignKey("ServerTemplateId")]
        public ServerTemplate ServerTemplate { get; set; }
    }
}
