using System;
using System.Runtime.Serialization;

namespace BRM.BL.Exceptions
{
    [Serializable]
    internal class ObjectAlreadyExistException : Exception
    {
        public ObjectAlreadyExistException()
        {
        }

        public ObjectAlreadyExistException(string message) : base(message)
        {
        }

        public ObjectAlreadyExistException(string message, Exception ex) : base(message)
        {
            Ex = ex;
        }

        protected ObjectAlreadyExistException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public Exception Ex { get; }
    }
}
