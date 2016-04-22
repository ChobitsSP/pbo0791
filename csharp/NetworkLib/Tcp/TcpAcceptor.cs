namespace NetworkLib.Tcp
{
    using NetworkLib;
    using NetworkLib.Utilities;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class TcpAcceptor : TcpNetworkWorker, IAcceptor, INetworkWorker
    {
        public event AcceptReactorDelegate OnAcceptNew;

        private TcpAcceptor(Socket socket, bool sync) : base(socket, sync)
        {
        }

        private void AcceptCallBack(IAsyncResult asyncResult)
        {
            Socket asyncState = asyncResult.AsyncState as Socket;
            try
            {
                Socket socket = asyncState.EndAccept(asyncResult);
                this.AcceptNext();
                this.OnAccept(new TcpReactor(socket, base._sync));
            }
            catch (SocketException exception)
            {
                this.OnAcceptFail(exception);
            }
        }

        private void AcceptNext()
        {
            try
            {
                if (base._sync)
                {
                    IAsyncResult asyncResult = base._socket.BeginAccept(null, base._socket);
                    base.AddAsyncBlock(asyncResult, base._socket, 1);
                }
                else
                {
                    base._socket.BeginAccept(new AsyncCallback(this.AcceptCallBack), base._socket);
                }
            }
            catch (SocketException exception)
            {
                this.OnAcceptFail(exception);
            }
        }

        protected override bool CompleteOperation(SocketNetworkWorker.AsyncBlock var)
        {
            if (var._operationFlag == 1)
            {
                this.AcceptCallBack(var._asyncResult);
                return true;
            }
            return false;
        }

        public static TcpAcceptor Create(bool sync, int port, int maxConnection)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.LingerState = new LingerOption(false, 0);
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(maxConnection);
                return new TcpAcceptor(socket, sync);
            }
            catch (SocketException exception)
            {
                Logger.LogError("Fail to create acceptor: {0}", new object[] { exception.Message });
                return null;
            }
        }

        private void HandleAccpetNewEvent(IReactor reactor)
        {
            if (this.OnAcceptNew != null)
            {
                this.OnAcceptNew(reactor);
            }
        }

        public void OnAccept(IReactor reactor)
        {
            Logger.LogInfo("Accepted new client", new object[0]);
            this.HandleAccpetNewEvent(reactor);
        }

        private void OnAcceptFail(SocketException socketError)
        {
            base.DelayError(socketError);
        }

        public override bool Start()
        {
            this.AcceptNext();
            return true;
        }
    }
}

