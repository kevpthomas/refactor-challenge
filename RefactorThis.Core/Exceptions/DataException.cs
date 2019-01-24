using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace RefactorThis.Core.Exceptions
{
    /// <summary>
    /// Represents an exception thrown for an error retrieving data.
    /// </summary>
    [Serializable]
    public class DataException : DbException
    {
        public DataException(string message): base(message)
        {
        }

        public DataException(string message,
            Exception innerException): base(message, innerException)
        {
        }

        protected DataException(SerializationInfo info,
            StreamingContext context): base(info, context)
        {
        }

    }
}