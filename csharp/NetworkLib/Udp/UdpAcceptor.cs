namespace NetworkLib.Udp
{
    using NetworkLib;
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class UdpAcceptor : UdpReactor, IAcceptor, INetworkWorker
    {
        private Dictionary<EndPoint, UdpReactor> _clients;
        private EndPoint _remoteEndPoint;

        public event AcceptReactorDelegate OnAcceptNew;

        private UdpAcceptor(Socket socket, bool sync, EndPoint localEndPoint) : base(socket, sync, localEndPoint, null)
        {
            this._remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            this._clients = new Dictionary<EndPoint, UdpReactor>();
        }

        public static UdpAcceptor Create(bool sync, int port)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                return new UdpAcceptor(socket, sync, new IPEndPoint(IPAddress.Any, port));
            }
            catch (SocketException exception)
            {
                Logger.LogError("Cannot start server at port : {0}", new object[] { port });
                Logger.LogException(exception);
                return null;
            }
        }

        private UdpReactor GetReactor(EndPoint endPoint, ref bool newCommer)
        {
            if (!this._clients.ContainsKey(endPoint))
            {
                UdpReactor reactor = new UdpReactor(null, base._sync, endPoint, this);
                this._clients[endPoint] = reactor;
                this.OnAccept(reactor);
                newCommer = true;
                return reactor;
            }
            newCommer = false;
            return this._clients[endPoint];
        }

        protected override void HandleReceived(byte[] buffer, EndPoint endPoint)
        {
            bool newCommer = false;
            this.GetReactor(endPoint, ref newCommer).PushReceive(buffer, 0, buffer.Length);
        }

        public void OnAccept(IReactor reactor)
        {
            Logger.LogInfo("Accepted new client", new object[0]);
            if (this.OnAcceptNew != null)
            {
                this.OnAcceptNew(reactor);
            }
        }
    }
}

