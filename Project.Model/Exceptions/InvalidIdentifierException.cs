using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Model.Exceptions
{
    public class InvalidIdentifierException : ApplicationException
    {
        public InvalidIdentifierException(string message) : base(message) { }
    }
}
