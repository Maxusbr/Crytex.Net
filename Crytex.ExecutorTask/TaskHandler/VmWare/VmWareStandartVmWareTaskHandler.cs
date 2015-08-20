﻿using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareStandartVmWareTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareStandartVmWareTaskHandler(StandartVmTask task, IVmWareControl vmWareControl) : base(task, vmWareControl) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Standart task vmware");
            return new TaskExecutionResult();
        }
    }
}
