using System;
using System.Runtime.Serialization;

namespace DTNL.LL.Logic.Exceptions
{
    public class NoLightsOnlineException : Exception
    {
        public NoLightsOnlineException()
        {
        }

        protected NoLightsOnlineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NoLightsOnlineException(string? message) : base(message)
        {
        }

        public NoLightsOnlineException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}