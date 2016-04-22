namespace NetworkLib.Tcp
{
    using NetworkLib;
    using NetworkLib.Utilities;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class TcpReactor : TcpNetworkWorker, IReactor, INetworkWorker
    {
        public event IOErrorDelegate OnIOError;

        public event NetworkErrorDelegate OnNetworkError;

        public event HandleReadBufferDelegate OnReceive;

        public TcpReactor(Socket socket) : base(socket)
        {
        }

        public TcpReactor(Socket socket, bool sync) : base(socket, sync)
        {
        }

        protected override bool CompleteOperation(SocketNetworkWorker.AsyncBlock var)
        {
            switch (var._operationFlag)
            {
                case 3:
                    this.ReadCallBack(var._asyncResult);
                    break;

                case 4:
                    this.SendCallBack(var._asyncResult);
                    break;

                default:
                    return false;
            }
            return true;
        }

        private void HandleIOErrorEvent(IOException error)
        {
            if (this.OnIOError != null)
            {
                this.OnIOError(error);
            }
        }

        private void HandleNetworkError(NetworkException error)
        {
            if (this.OnNetworkError != null)
            {
                this.OnNetworkError(error);
            }
        }

        private void HandleReceiveEvent(ByteArray buffer)
        {
            if (this.OnReceive != null)
            {
                this.OnReceive(buffer);
            }
        }

        protected void ProcessBuffer()
        {
            for (ByteArray array = base.ReceiveBuffer.ReadByteArray(); array != null; array = base.ReceiveBuffer.ReadByteArray())
            {
                this.HandleReceiveEvent(array);
            }
            base.ReceiveBuffer.Shrink();
        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            Socket asyncState = asyncResult.AsyncState as Socket;
            try
            {
                int count = asyncState.EndReceive(asyncResult);
                if (count > 0)
                {
                    base.ReceiveBuffer.Append(base._buffer, 0, count);
                    this.ReadNext();
                    this.ProcessBuffer();
                }
                else
                {
                    Logger.LogWarn("Read from network result in {0}", new object[] { count });
                    this.OnNetworkError(new NetworkException("cannot read from network."));
                }
            }
            catch (IOException exception)
            {
                this.HandleIOErrorEvent(exception);
                base.DelayError(exception);
            }
            catch (SocketException exception2)
            {
                this.HandleNetworkError(new TcpNetworkException(exception2));
                base.DelayError(exception2);
            }
        }

        protected void ReadNext()
        {
            if (base._socket != null)
            {
                try
                {
                    if (base._sync)
                    {
                        IAsyncResult asyncResult = base._socket.BeginReceive(base._buffer, 0, base._buffer.Length, SocketFlags.None, null, base._socket);
                        base.AddAsyncBlock(asyncResult, base._socket, 3);
                    }
                    else
                    {
                        base._socket.BeginReceive(base._buffer, 0, base._buffer.Length, SocketFlags.None, new AsyncCallback(this.ReadCallBack), base._socket);
                    }
                }
                catch (SocketException exception)
                {
                    this.HandleNetworkError(new TcpNetworkException(exception));
                }
                catch (Exception exception2)
                {
                    base.DelayError(exception2);
                }
            }
        }

        public void Send(ByteArray byteArray)
        {
            if (base._socket != null)
            {
                try
                {
                    if (base._sync)
                    {
                        IAsyncResult asyncResult = base._socket.BeginSend(byteArray.Buffer, 0, byteArray.Length, SocketFlags.None, null, base._socket);
                        base.AddAsyncBlock(asyncResult, base._socket, 4);
                    }
                    else
                    {
                        base._socket.BeginSend(byteArray.Buffer, 0, byteArray.Length, SocketFlags.None, new AsyncCallback(this.SendCallBack), base._socket);
                    }
                }
                catch (SocketException exception)
                {
                    this.HandleNetworkError(new TcpNetworkException(exception));
                }
                catch (Exception exception2)
                {
                    base.DelayError(exception2);
                }
            }
        }

        private void SendCallBack(IAsyncResult asyncResult)
        {
            Socket asyncState = asyncResult.AsyncState as Socket;
            try
            {
                asyncState.EndSend(asyncResult);
            }
            catch (SocketException exception)
            {
                this.HandleNetworkError(new TcpNetworkException(exception));
                base.DelayError(exception);
            }
        }

        public override bool Start()
        {
            base.ReceiveBuffer.BypassHeader();
            this.ReadNext();
            return true;
        }

        public IPAddress RemoteAddress
        {
            get
            {
                return (base._socket.RemoteEndPoint as IPEndPoint).Address;
            }
        }
    }
}

