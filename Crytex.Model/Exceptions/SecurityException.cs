using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Exceptions
{
    public class SecurityException : ApplicationException
    {
        public SecurityException(string message) : base(message) { }
    }
}
