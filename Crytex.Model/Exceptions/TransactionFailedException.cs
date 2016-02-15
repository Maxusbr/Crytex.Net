using System;

namespace Crytex.Model.Exceptions
{
    public class TransactionFailedException : ApplicationException
    {
        public enum TypeError
        {
            NotEnough
        }
        public TransactionFailedException(string message) : base(message) { }

        public TypeError Type
        {
            get;
            set;
        }
        public TransactionFailedException(string message, TransactionFailedException.TypeError type) : base(message)
        {
            this.Type = type;
        }

    }

    
}
