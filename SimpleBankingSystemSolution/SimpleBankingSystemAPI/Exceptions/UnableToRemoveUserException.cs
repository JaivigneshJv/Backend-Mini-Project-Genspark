using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class UnableToRemoveUserException : Exception
    {
        public UnableToRemoveUserException()
        {
        }

        public UnableToRemoveUserException(string? message) : base(message)
        {
        }

        public UnableToRemoveUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnableToRemoveUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}