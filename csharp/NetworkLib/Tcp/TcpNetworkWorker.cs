namespace NetworkLib.Tcp
{
    using NetworkLib;
    using System;
    using System.Net.Sockets;

    public abstract class TcpNetworkWorker : SocketNetworkWorker
    {
        public TcpNetworkWorker() : this(true)
        {
        }

        public TcpNetworkWorker(bool sync) : this(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), sync)
        {
        }

        public TcpNetworkWorker(Socket socket) : this(socket, true)
        {
        }

        public TcpNetworkWorker(Socket socket, bool sync) : base(socket, sync)
        {
        }
    }
}

