using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class DueDateException : Exception
    {
        public DueDateException()
        {
        }

        public DueDateException(string? message) : base(message)
        {
        }

        public DueDateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DueDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}