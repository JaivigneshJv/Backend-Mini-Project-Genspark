using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class SamePasswordException : Exception
    {
        public SamePasswordException()
        {
        }

        public SamePasswordException(string? message) : base(message)
        {
        }

        public SamePasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SamePasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}