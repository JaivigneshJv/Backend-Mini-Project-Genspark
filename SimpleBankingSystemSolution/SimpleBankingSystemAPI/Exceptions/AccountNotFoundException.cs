﻿using System.Runtime.Serialization;

namespace SimpleBankingSystemAPI.Exceptions
{
    [Serializable]
    internal class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
        {
        }

        public AccountNotFoundException(string? message) : base(message)
        {
        }

        public AccountNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}