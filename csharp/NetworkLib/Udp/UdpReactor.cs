namespace NetworkLib.Udp
{
    using NetworkLib;
    using NetworkLib.Utilities;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class UdpReactor : UdpNetworkWorker, IReactor, INetworkWorker
    {
        private UdpAcceptor _acceptor;
        private System.Net.EndPoint _endPoint;

        public event IOErrorDelegate OnIOError;

        public event NetworkErrorDelegate OnNetworkError;

        public event HandleReadBufferDelegate OnReceive;

        public UdpReactor(Socket socket, bool sync, System.Net.EndPoint endPoint, UdpAcceptor acceptor) : base(socket, sync)
        {
            this._endPoint = endPoint;
            this._acceptor = acceptor;
        }

        protected override bool CompleteOperation(SocketNetworkWorker.AsyncBlock var)
        {
            switch (var._operationFlag)
            {
                case 3:
                    this.ReceiveCallBack(var._asyncResult);
                    break;

                case 4:
                    this.SendCallBack(var._asyncResult);
                    break;

                default:
                    return false;
            }
            return true;
        }

        private void HandleIOErrorEvent(IOException IOError)
        {
            if (this.OnIOError != null)
            {
                this.OnIOError(IOError);
            }
        }

        private void HandleNetworkErrorEvent(SocketException socketError)
        {
            if (this.OnNetworkError != null)
            {
                this.OnNetworkError(new UdpNetworkException(socketError));
            }
        }

        protected void HandlePackages()
        {
            for (ByteArray array = base._receiveBuffer.ReadByteArray(); array != null; array = base._receiveBuffer.ReadByteArray())
            {
                this.HandleReceiveEvent(array);
            }
            base._receiveBuffer.Shrink();
        }

        protected virtual void HandleReceived(byte[] buffer, System.Net.EndPoint endPoint)
        {
            if (this._acceptor == null)
            {
                base._receiveBuffer.Append(buffer, 0, buffer.Length);
                this.HandlePackages();
            }
        }

        private void HandleReceiveEvent(ByteArray buffer)
        {
            if (this.OnReceive != null)
            {
                this.OnReceive(buffer);
            }
        }

        protected void OnReceiveFailed(SocketException socketError)
        {
            base.DelayError(socketError);
        }

        internal void PushReceive(byte[] buffer, int offset, int len)
        {
            base._receiveBuffer.Append(buffer, offset, len);
            this.HandlePackages();
        }

        private void ReceiveCallBack(IAsyncResult asyncResult)
        {
            UdpStateObject asyncState = asyncResult.AsyncState as UdpStateObject;
            try
            {
                int count = asyncState._socket.EndReceiveFrom(asyncResult, ref asyncState._endPoint);
                if (count > 0)
                {
                    byte[] dst = new byte[count];
                    Buffer.BlockCopy(base._buffer, 0, dst, 0, count);
                    this.HandleReceived(dst, asyncState._endPoint);
                    this.ReceiveNext();
                }
                else
                {
                    Logger.LogWarn("Read from network result in {0}", new object[] { count });
                    this.OnNetworkError(new NetworkException("cannot read from network."));
                }
            }
            catch (SocketException exception)
            {
                this.OnReceiveFailed(exception);
            }
        }

        protected void ReceiveNext()
        {
            System.Net.EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpStateObject state = new UdpStateObject(base._socket, endPoint);
            try
            {
                if (base._sync)
                {
                    IAsyncResult asyncResult = base._socket.BeginReceiveFrom(base._buffer, 0, base._buffer.Length, SocketFlags.None, ref endPoint, null, state);
                    base.AddAsyncBlock(asyncResult, base._socket, 3);
                }
                else
                {
                    base._socket.BeginReceiveFrom(base._buffer, 0, base._buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(this.ReceiveCallBack), state);
                }
            }
            catch (SocketException exception)
            {
                this.OnReceiveFailed(exception);
            }
        }

        public void Send(ByteArray byteArray)
        {
            if (this._acceptor != null)
            {
                this._acceptor.SendTo(byteArray, this._endPoint);
            }
            else
            {
                this.SendTo(byteArray, this._endPoint);
            }
        }

        private void SendCallBack(IAsyncResult asyncResult)
        {
            UdpStateObject asyncState = asyncResult.AsyncState as UdpStateObject;
            try
            {
                asyncState._socket.EndSendTo(asyncResult);
            }
            catch (SocketException exception)
            {
                base.DelayError(exception);
            }
        }

        protected void SendTo(ByteArray byteArray, System.Net.EndPoint endPoint)
        {
            UdpStateObject state = new UdpStateObject(base._socket, endPoint, null);
            try
            {
                if (base._sync)
                {
                    IAsyncResult asyncResult = base._socket.BeginSendTo(byteArray.Buffer, 0, byteArray.Length, SocketFlags.None, endPoint, null, state);
                    base.AddAsyncBlock(asyncResult, base._socket, 4);
                }
                else
                {
                    base._socket.BeginSendTo(byteArray.Buffer, 0, byteArray.Length, SocketFlags.None, endPoint, new AsyncCallback(this.SendCallBack), state);
                }
            }
            catch (SocketException exception)
            {
                Logger.LogError("Error in send : {0}", new object[] { exception.Message });
            }
        }

        public override bool Start()
        {
            base._receiveBuffer.BypassHeader();
            if (this._acceptor == null)
            {
                this.ReceiveNext();
            }
            return true;
        }

        public System.Net.EndPoint EndPoint
        {
            get
            {
                return this._endPoint;
            }
            set
            {
                this._endPoint = value;
            }
        }

        public IPAddress RemoteAddress
        {
            get
            {
                return (this._endPoint as IPEndPoint).Address;
            }
        }
    }
}

