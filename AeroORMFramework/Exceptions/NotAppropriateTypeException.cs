using System;
using System.Runtime.Serialization;

namespace AeroORMFramework
{
    /// <summary>
    /// Exception which is thrown when the input type when creation 
    /// of a new table is proceeding is wrong
    /// </summary>
    internal class NotAppropriateTypeException : SystemException
    {
        public NotAppropriateTypeException() { }
        public NotAppropriateTypeException(string message) : base(message) { }
        public NotAppropriateTypeException(string message, Exception innerException) : base(message, innerException) { }
        protected NotAppropriateTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
