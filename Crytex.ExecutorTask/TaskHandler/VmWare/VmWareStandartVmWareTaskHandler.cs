﻿using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareStandartVmWareTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareStandartVmWareTaskHandler(StandartVmTask task, IVmWareControl vmWareControl, string hostName)
            : base(task, vmWareControl, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Standart task vmware");
            return new TaskExecutionResult();
        }
    }
}
