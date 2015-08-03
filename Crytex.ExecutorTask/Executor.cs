using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
