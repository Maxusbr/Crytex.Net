using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.ExecutorTask.alt
{
    public class TaskHandlerFactory
    {
        public ITaskHandler GetHandler(BaseTask task)
        {
            return new FakeTaskHandler();
        }
    }
}
