namespace NetworkLib.Udp
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    internal class UdpStateObject
    {
        public EndPoint _endPoint;
        public UdpReactor _reactor;
        public Socket _socket;

        public UdpStateObject(Socket socket, EndPoint endPoint) : this(socket, endPoint, null)
        {
        }

        public UdpStateObject(Socket socket, EndPoint endPoint, UdpReactor reactor)
        {
            this._socket = socket;
            this._endPoint = endPoint;
            this._reactor = reactor;
        }
    }
}

