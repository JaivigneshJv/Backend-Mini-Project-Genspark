using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class TransactionVerificationNotFoundException : Exception
    {
        public TransactionVerificationNotFoundException()
        {
        }

        public TransactionVerificationNotFoundException(string? message) : base(message)
        {
        }

        public TransactionVerificationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TransactionVerificationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}