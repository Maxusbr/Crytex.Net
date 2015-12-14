using System;
using System.Collections.Generic;
using HyperVRemote;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class CreateVmResult
    {
        public Guid MachineGuid { get; set; }
        public IEnumerable<NetAdapter> NetworkAdapters { get; internal set; }
    }
}
