using System;
using System.Runtime.Serialization;

namespace AeroORMFramework
{
    /// <summary>
    /// Exception which is thrown when an input parameter is wrong
    /// </summary>
    public class NotAppropriateParamException : SystemException
    {
        public NotAppropriateParamException() { }
        public NotAppropriateParamException(string message) : base(message) { }
        public NotAppropriateParamException(string message, Exception innerException) 
            : base(message, innerException) { }
        protected NotAppropriateParamException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }
    }
}
