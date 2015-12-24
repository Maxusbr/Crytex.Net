using System;

namespace Crytex.Model.Exceptions
{
    public class OperationNotSupportedException : ApplicationException
    {
        public OperationNotSupportedException(string message) : base(message) { }
    }
}
