using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Crytex.ExecutorTask.Task;

namespace Crytex.ExecutorTask.Hyper_V
{
   public  interface IHyperVExecutor
   {
       void CreateVm(CreateVMTask task);
   }
}
