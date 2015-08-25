using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Model.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message) { }
    }
}
