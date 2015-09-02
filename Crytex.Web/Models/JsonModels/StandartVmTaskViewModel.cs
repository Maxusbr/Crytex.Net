using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class StandartVmTaskViewModel
    {
        public int Id { get; set; }

        public int VmId { get; set; }

        public TypeStandartVmTask TaskType { get; set; }

        public TypeVirtualization Virtualization { get; set; }

        public StatusTask StatusTask { get; set; }

    }
}