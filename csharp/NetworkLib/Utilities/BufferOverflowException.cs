namespace NetworkLib.Utilities
{
    using System;
    using System.Runtime.Serialization;

    public class BufferOverflowException : Exception
    {
        public BufferOverflowException()
        {
        }

        public BufferOverflowException(string message) : base(message)
        {
        }

        public BufferOverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BufferOverflowException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BufferOverflowException(string format, params object[] args) : base(string.Format(format, args))
        {
        }
    }
}

