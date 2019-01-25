using System;
using System.Runtime.Serialization;

namespace BackSide2.BL.Exceptions
{
    [Serializable]
    internal class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException()
        {
        }

        public ObjectNotFoundException(string message) : base(message)
        {
        }

        public ObjectNotFoundException(string message, Exception ex) : base(message)
        {
            Ex = ex;
        }

        protected ObjectNotFoundException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public Exception Ex { get; }
    }
}
