using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Exceptions
{
    public class CreateVmException : ApplicationException
    {
        public CreateVmException(string message) : base(message) { }
        public CreateVmException(string message, Exception innerException) : base(message, innerException) { }
    }
}
