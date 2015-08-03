using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.ExecutorTask.Task;

namespace Crytex.ExecutorTask.Hyper_V
{
    public class HyperVFakeExecutor : IHyperVExecutor
    {
        public void CreateVm(CreateVMTask task)
        {
            throw new NotImplementedException();
        }
    }
}
