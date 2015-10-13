using Crytex.ExecutorTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Background
{
    public class FakeTaskManager : ITaskManager
    {
        public void RunTasks()
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskQueues()
        {
            throw new NotImplementedException();
        }
    }
}
