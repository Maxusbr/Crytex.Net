using System;
using Crytex.ExecutorTask.Hyper_V;
using Crytex.ExecutorTask.VmWare;

namespace Crytex.ExecutorTask
{
    
    public class Executor
    {
        private IHyperVExecutor _hyperVExecutor;
   
        private IWmWareExecutor _vmWareExecutor;

        public Executor(IHyperVExecutor iHyperVExecutor, IWmWareExecutor vmWareExecutor)
        {
            _vmWareExecutor = vmWareExecutor;
            _hyperVExecutor = iHyperVExecutor;
        }


        public void RunCreateVm(Int32 id)
        {
            
        }
    }
}
