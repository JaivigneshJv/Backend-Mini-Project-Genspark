using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class PendingAccountClosingNotFoundException : Exception
    {
        public PendingAccountClosingNotFoundException()
        {
        }

        public PendingAccountClosingNotFoundException(string? message) : base(message)
        {
        }

        public PendingAccountClosingNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PendingAccountClosingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}