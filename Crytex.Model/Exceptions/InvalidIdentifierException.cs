using System;

namespace Crytex.Model.Exceptions
{
    public class InvalidIdentifierException : ApplicationException
    {
        public InvalidIdentifierException(string message) : base(message) { }
    }
}
