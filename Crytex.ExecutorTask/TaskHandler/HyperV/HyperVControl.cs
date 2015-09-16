using Crytex.Model.Models;
using HyperVRemote;
namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVControl : IHyperVControl
    {
        private IHyperVProvider _hyperVProvider;
        
        public HyperVControl(IHyperVProvider hyperVProvider)
        {
            this._hyperVProvider = hyperVProvider;
        }

        public void CreateVm(CreateVmTask task)
        {
            this._hyperVProvider.Connect();
            this._hyperVProvider.CreateMachine(task.Name);
        }
    }
}
