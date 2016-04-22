namespace NetworkLib.Tcp
{
    using NetworkLib;
    using System;
    using System.Net.Sockets;
    using System.Runtime.Serialization;

    public class TcpNetworkException : NetworkException
    {
        private SocketException _socketError;

        public TcpNetworkException()
        {
        }

        public TcpNetworkException(Exception innerException) : this("Network Exception", innerException)
        {
        }

        public TcpNetworkException(string message) : base(message)
        {
        }

        public TcpNetworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TcpNetworkException(string message, Exception innerException) : base(message, innerException)
        {
            if (innerException is SocketException)
            {
                this._socketError = innerException as SocketException;
            }
        }

        public TcpNetworkException(string format, params object[] args) : base(string.Format(format, args))
        {
        }

        public SocketException SocketError
        {
            get
            {
                return this._socketError;
            }
            set
            {
                this._socketError = value;
            }
        }
    }
}

