namespace NetworkLib
{
    using System;
    using System.Runtime.Serialization;

    public class NetworkException : Exception
    {
        public NetworkException()
        {
        }

        public NetworkException(string message) : base(message)
        {
        }

        public NetworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NetworkException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

