using System;
using System.Runtime.Serialization;

namespace DTNL.LL.Logic.Exceptions
{
    public class InvalidApiKeyException : Exception
    {
        public InvalidApiKeyException()
        {
        }

        protected InvalidApiKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidApiKeyException(string? message) : base(message)
        {
        }

        public InvalidApiKeyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}