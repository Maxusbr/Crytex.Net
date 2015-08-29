using System;

namespace Crytex.Model.Exceptions
{
    public class TaskOperationException : ApplicationException
    {
        public TaskOperationException(string message) : base(message) { }
    }
}
