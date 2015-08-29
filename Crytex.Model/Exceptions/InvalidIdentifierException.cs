using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Model.Exceptions
{
    public class InvalidIdentifierException : ApplicationException
    {
        public InvalidIdentifierException(string message) : base(message) { }
    }
}
