using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class AccountNotActivedException : Exception
    {
        public AccountNotActivedException()
        {
        }

        public AccountNotActivedException(string? message) : base(message)
        {
        }

        public AccountNotActivedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountNotActivedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}