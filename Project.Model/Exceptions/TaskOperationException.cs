using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Model.Exceptions
{
    public class TaskOperationException : ApplicationException
    {
        public TaskOperationException(string message) : base(message) { }
    }
}
