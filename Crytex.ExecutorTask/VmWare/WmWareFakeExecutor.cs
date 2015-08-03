using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.ExecutorTask.Hyper_V;
using Crytex.ExecutorTask.Task;

namespace Crytex.ExecutorTask.VmWare
{
    public class WmWareFakeExecutor : IWmWareExecutor
    {
        public void CreateVm(CreateVMTask task)
        {
            throw new NotImplementedException();
        }
    }
}
