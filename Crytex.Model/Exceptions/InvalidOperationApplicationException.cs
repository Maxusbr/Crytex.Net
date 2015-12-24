using System;

namespace Crytex.Model.Exceptions
{
    public class InvalidOperationApplicationException : ApplicationException
    {
        public InvalidOperationApplicationException(string message) : base(message) { }
    }
}
