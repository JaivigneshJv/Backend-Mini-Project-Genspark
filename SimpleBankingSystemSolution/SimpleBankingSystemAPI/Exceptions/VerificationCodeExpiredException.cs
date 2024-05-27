using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class VerificationCodeExpiredException : Exception
    {
        public VerificationCodeExpiredException()
        {
        }

        public VerificationCodeExpiredException(string? message) : base(message)
        {
        }

        public VerificationCodeExpiredException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected VerificationCodeExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}