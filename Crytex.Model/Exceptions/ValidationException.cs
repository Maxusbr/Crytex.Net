using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Model.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message) { }
    }
}
