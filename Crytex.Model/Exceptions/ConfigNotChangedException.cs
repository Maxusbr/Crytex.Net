using System;

namespace Crytex.Model.Exceptions
{
    public class ConfigNotChangedException : ApplicationException
    {
        public ConfigNotChangedException(string message) : base(message) { }
    }
}
