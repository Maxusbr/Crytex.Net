﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.alt
{
    public interface ITaskHandler
    {
        TaskExecutionResult Execute();
    }
}
