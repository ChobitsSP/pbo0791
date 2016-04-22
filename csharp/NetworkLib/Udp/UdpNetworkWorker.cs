namespace NetworkLib.Udp
{
    using NetworkLib;
    using System;
    using System.Net.Sockets;

    public abstract class UdpNetworkWorker : SocketNetworkWorker
    {
        public UdpNetworkWorker() : this(true)
        {
        }

        public UdpNetworkWorker(bool sync) : this(new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp), true)
        {
        }

        public UdpNetworkWorker(Socket socket) : this(socket, true)
        {
        }

        public UdpNetworkWorker(Socket socket, bool sync) : base(socket, sync)
        {
        }
    }
}

