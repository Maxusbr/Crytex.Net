using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class UpdateMachineStatusOptions
    {
        public TypeChangeStatus Status { get; internal set; }
        public string SubscriptionId { get; internal set; }
    }
}