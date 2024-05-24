using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class InvalidEmailVerificationCode : Exception
    {
        public InvalidEmailVerificationCode()
        {
        }

        public InvalidEmailVerificationCode(string? message) : base(message)
        {
        }

        public InvalidEmailVerificationCode(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidEmailVerificationCode(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}