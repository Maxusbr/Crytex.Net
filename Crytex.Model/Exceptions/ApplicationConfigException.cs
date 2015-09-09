using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Model.Exceptions
{
    public class ApplicationConfigException : ApplicationException
    {
        public ApplicationConfigException(string message) : base(message) { }
    }
}
