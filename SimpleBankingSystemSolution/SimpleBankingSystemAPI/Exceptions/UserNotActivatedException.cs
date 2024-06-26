﻿using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class UserNotActivatedException : Exception
    {
        public UserNotActivatedException()
        {
        }

        public UserNotActivatedException(string? message) : base(message)
        {
        }

        public UserNotActivatedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserNotActivatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}