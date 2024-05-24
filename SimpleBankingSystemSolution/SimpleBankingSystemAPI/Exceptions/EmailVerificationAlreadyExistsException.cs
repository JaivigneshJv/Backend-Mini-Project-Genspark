using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class EmailVerificationAlreadyExistsException : Exception
    {
        public EmailVerificationAlreadyExistsException()
        {
        }

        public EmailVerificationAlreadyExistsException(string? message) : base(message)
        {
        }

        public EmailVerificationAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmailVerificationAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}