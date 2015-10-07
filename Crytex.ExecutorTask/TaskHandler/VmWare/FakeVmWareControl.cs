namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class FakeVmWareControl : IVmWareControl
    {
        public System.Guid CreateVm(Model.Models.CreateVmTask task)
        {
            throw new System.NotImplementedException();
        }


        public void UpdateVm(Model.Models.UpdateVmTask updateVmTask)
        {
            throw new System.NotImplementedException();
        }

        public void StartVm(string machineName)
        {
            throw new System.NotImplementedException();
        }

        public void StopVm(string machineName)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveVm(string machineName)
        {
            throw new System.NotImplementedException();
        }
    }
}
