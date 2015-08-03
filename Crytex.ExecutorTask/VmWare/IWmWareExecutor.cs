using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.ExecutorTask.Task;

namespace Crytex.ExecutorTask.VmWare
{
    public interface IWmWareExecutor
    {
        void CreateVm(CreateVMTask task);
    }
}
