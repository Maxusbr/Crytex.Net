using System;

namespace Crytex.Model.Exceptions
{
    public class TransactionFailedException : ApplicationException
    {
        public TransactionFailedException(string message) : base(message) { }
    }
}
