using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class TransactionAlreadyInProgressException : Exception
    {
        public TransactionAlreadyInProgressException()
        {
        }

        public TransactionAlreadyInProgressException(string? message) : base(message)
        {
        }

        public TransactionAlreadyInProgressException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TransactionAlreadyInProgressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}