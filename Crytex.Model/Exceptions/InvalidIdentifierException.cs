using System;

namespace Crytex.Model.Exceptions
{
    public class InvalidIdentifierException : ApplicationException
    {
        public InvalidIdentifierException(string message) : base(message) { }

        public InvalidIdentifierException(string message, Exception innerException) : base(message, innerException) { }
    }
}
