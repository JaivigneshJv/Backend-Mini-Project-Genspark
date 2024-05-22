using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class InvalidCredentialException : Exception
    {
        public InvalidCredentialException()
        {
        }

        public InvalidCredentialException(string? message) : base(message)
        {
        }

        public InvalidCredentialException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidCredentialException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}