using System;
using System.Runtime.Serialization;

namespace DTNL.LL.Logic.Exceptions
{
    public class GroupNotFoundException : Exception
    {
        public GroupNotFoundException()
        {
        }

        protected GroupNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GroupNotFoundException(string? message) : base(message)
        {
        }

        public GroupNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}