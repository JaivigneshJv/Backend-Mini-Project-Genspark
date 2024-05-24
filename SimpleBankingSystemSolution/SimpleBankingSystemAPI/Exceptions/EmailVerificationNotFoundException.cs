using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class EmailVerificationNotFoundException : Exception
    {
        public EmailVerificationNotFoundException()
        {
        }

        public EmailVerificationNotFoundException(string? message) : base(message)
        {
        }

        public EmailVerificationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmailVerificationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}