using System;

namespace Crytex.Model.Exceptions
{
    public class DbUpdateApplicationException : ApplicationException
    {
        public DbUpdateApplicationException(string message) : base(message)
        {
        }
    }
}
