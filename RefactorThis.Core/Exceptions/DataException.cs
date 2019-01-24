using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace RefactorThis.Core.Exceptions
{
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