using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class GameServerConfiguration : BaseEntity
    {
        public string GameName { get; set; }
        public int ServerTemplateId { get; set; }

        [ForeignKey("ServerTemplateId")]
        public ServerTemplate ServerTemplate { get; set; }
        // Стоимость процессора
        public decimal Processor1 { get; set; }
        // Стоимость памяти
        public decimal RAM512 { get; set; }
        // Стоимость слота
        public decimal Slot { get; set; }
    }
}
