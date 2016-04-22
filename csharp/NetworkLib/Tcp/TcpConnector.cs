namespace NetworkLib.Tcp
{
    using NetworkLib;
    using NetworkLib.Utilities;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class TcpConnector : TcpNetworkWorker, IConnector, INetworkWorker
    {
        private bool _connected;
        private int _port;
        protected TcpReactor _reactor;
        private string _serverName;

        public event NetworkEventDelegate OnConnected;

        public event NetworkErrorDelegate OnConnectFail;

        public TcpConnector(bool sync, string serverName, int port) : base(sync)
        {
            this._serverName = serverName;
            this._port = port;
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
            IPAddress[] serverIP = this.GetServerIP(this._serverName);
            try
            {
                if (base._sync)
                {
                    IAsyncResult asyncResult = base._socket.BeginConnect(serverIP, this._port, null, base._socket);
                    base.AddAsyncBlock(asyncResult, base._socket, 2);
                }
                else
                {
                    base._socket.BeginConnect(serverIP, this._port, new AsyncCallback(this.ConnectCallBack), base._socket);
                }
            }
            catch (SocketException exception)
            {
                this.ConnectFail(exception);
            }
        }

        private void ConnectCallBack(IAsyncResult asyncResult)
        {
            Socket asyncState = asyncResult.AsyncState as Socket;
            try
            {
                asyncState.EndConnect(asyncResult);
                this.ConnectSuccess();
            }
            catch (SocketException exception)
            {
                this.ConnectFail(exception);
            }
        }

        private void ConnectFail(SocketException socketError)
        {
            Logger.LogError("Fail to connect server: {0}", new object[] { socketError.Message });
            this.HandleConnectFailEvent(socketError);
        }

        private void ConnectSuccess()
        {
            Logger.LogInfo("Connection success at {0}:{1}", new object[] { this._serverName, this._port });
            this._reactor = new TcpReactor(base._socket, base._sync);
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
                this.OnConnectFail(new TcpNetworkException(socketError));
            }
        }

        public void Reconnet()
        {
            if (this._reactor != null)
            {
                this.Connect();
            }
        }

        public override bool Start()
        {
            this.Connect();
            return true;
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

