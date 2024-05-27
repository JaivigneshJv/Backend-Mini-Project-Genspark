using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class InvalidRepaymentAmountException : Exception
    {
        public InvalidRepaymentAmountException()
        {
        }

        public InvalidRepaymentAmountException(string? message) : base(message)
        {
        }

        public InvalidRepaymentAmountException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidRepaymentAmountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}