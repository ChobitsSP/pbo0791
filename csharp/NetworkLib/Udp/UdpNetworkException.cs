namespace NetworkLib.Udp
{
    using NetworkLib;
    using System;
    using System.Net.Sockets;
    using System.Runtime.Serialization;

    public class UdpNetworkException : NetworkException
    {
        private SocketException _socketError;

        public UdpNetworkException()
        {
        }

        public UdpNetworkException(Exception innerException) : this("Network Exception", innerException)
        {
        }

        public UdpNetworkException(string message) : base(message)
        {
        }

        public UdpNetworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UdpNetworkException(string message, Exception innerException) : base(message, innerException)
        {
            if (innerException is SocketException)
            {
                this._socketError = innerException as SocketException;
            }
        }

        public UdpNetworkException(string format, params object[] args) : base(string.Format(format, args))
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

