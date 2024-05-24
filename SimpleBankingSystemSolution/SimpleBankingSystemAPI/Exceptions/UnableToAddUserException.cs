using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class UnableToAddUserException : Exception
    {
        public UnableToAddUserException()
        {
        }

        public UnableToAddUserException(string? message) : base(message)
        {
        }

        public UnableToAddUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnableToAddUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}