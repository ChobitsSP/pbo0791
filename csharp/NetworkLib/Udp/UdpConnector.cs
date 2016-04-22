namespace NetworkLib.Udp
{
    using NetworkLib;
    using NetworkLib.Utilities;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class UdpConnector : UdpNetworkWorker, IConnector, INetworkWorker
    {
        private bool _connected;
        private int _port;
        private UdpReactor _reactor;
        private string _serverName;

        public event NetworkEventDelegate OnConnected;

        public event NetworkErrorDelegate OnConnectFail;

        public UdpConnector(string serverName, int serverPort, bool sync) : base(sync)
        {
            this._serverName = serverName;
            this._port = serverPort;
        }

        protected override bool CompleteOperation(SocketNetworkWorker.AsyncBlock var)
        {
            if (var._operationFlag == 2)
            {
                this.ConnectCallBack(var._asyncResult);
                return true;
            }
            return false;
        }

        public void Connect()
        {
            try
            {
                if (base._sync)
                {
                    IAsyncResult asyncResult = base.Socket.BeginConnect(this.GetServerIP(this._serverName), this._port, null, base._socket);
                    base.AddAsyncBlock(asyncResult, base._socket, 2);
                }
                else
                {
                    base.Socket.BeginConnect(this.GetServerIP(this._serverName), this._port, new AsyncCallback(this.ConnectCallBack), base._socket);
                }
            }
            catch (SocketException exception)
            {
                this.HandleConnectFailEvent(exception);
            }
        }

        public void ConnectCallBack(IAsyncResult asyncResult)
        {
            try
            {
                base._socket.EndConnect(asyncResult);
                this.ConnectSuccess();
            }
            catch (SocketException exception)
            {
                this.HandleConnectFailEvent(exception);
            }
        }

        public void ConnectFail(SocketException socketError)
        {
            Logger.LogError("Fail to connect server: {0}", new object[] { socketError.Message });
            this.HandleConnectFailEvent(socketError);
        }

        public void ConnectSuccess()
        {
            Logger.LogInfo("Connection success at {0}:{1}", new object[] { this._serverName, this._port });
            this._reactor = new UdpReactor(base._socket, base._sync, base._socket.RemoteEndPoint, null);
            this._connected = true;
            this.HandleConnectedEvent();
        }

        private IPAddress[] GetServerIP(string serverName)
        {
            try
            {
                IPAddress address = IPAddress.Parse(serverName);
                return new IPAddress[] { address };
            }
            catch (FormatException)
            {
                return Dns.GetHostAddresses(serverName);
            }
        }

        private void HandleConnectedEvent()
        {
            if (this.OnConnected != null)
            {
                this.OnConnected();
            }
        }

        private void HandleConnectFailEvent(SocketException socketError)
        {
            if (this.OnConnectFail != null)
            {
                this.OnConnectFail(new UdpNetworkException(socketError));
            }
        }

        public void Reconnect()
        {
            if (this._reactor != null)
            {
                this.Connect();
            }
        }

        public override bool Start()
        {
            this.StartWithNoConnection();
            return true;
        }

        public void StartWithNoConnection()
        {
            IPEndPoint endPoint = new IPEndPoint(this.GetServerIP(this._serverName)[0], this._port);
            Logger.LogInfo("Connection success at {0}:{1}", new object[] { this._serverName, this._port });
            this._reactor = new UdpReactor(base._socket, base._sync, endPoint, null);
            this._connected = true;
            this.HandleConnectedEvent();
        }

        public bool Connected
        {
            get
            {
                return this._connected;
            }
        }

        public IReactor Reactor
        {
            get
            {
                return this._reactor;
            }
        }
    }
}

