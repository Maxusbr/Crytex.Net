using Crytex.Model.Models;
namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public interface IHyperVControl
    {
        void CreateVm(CreateVmTask task);
    }
}
