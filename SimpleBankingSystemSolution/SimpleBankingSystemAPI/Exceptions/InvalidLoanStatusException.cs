using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class InvalidLoanStatusException : Exception
    {
        public InvalidLoanStatusException()
        {
        }

        public InvalidLoanStatusException(string? message) : base(message)
        {
        }

        public InvalidLoanStatusException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidLoanStatusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}