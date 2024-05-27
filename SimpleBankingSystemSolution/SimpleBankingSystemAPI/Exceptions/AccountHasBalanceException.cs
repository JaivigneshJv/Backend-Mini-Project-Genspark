using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class AccountHasBalanceException : Exception
    {
        public AccountHasBalanceException()
        {
        }

        public AccountHasBalanceException(string? message) : base(message)
        {
        }

        public AccountHasBalanceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountHasBalanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}