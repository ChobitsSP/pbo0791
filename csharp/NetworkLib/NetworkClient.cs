namespace NetworkLib
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Tcp;
    public class NetworkClient : NetworkWorker
    {
        private List<ByteArray> _backupPackages;
        private List<ByteArray> _bufferedPackages;
        private IConnector _connecter;
        protected IProtocolInterpreter _interpreter;
        private IReactor _reactor;

        public event NetworkEventDelegate OnConnected;

        public event NetworkErrorDelegate OnConnectFail;

        public event NetworkEventDelegate OnDisconnected;

        protected NetworkClient() : this(null, new TcpNetworkStrategy())
        {
        }

        protected NetworkClient(INetworkStrategy networkStrategy) : this(null, networkStrategy)
        {
        }

        public NetworkClient(IProtocolInterpreter interpreter) : this(interpreter, new TcpNetworkStrategy())
        {
        }

        public NetworkClient(IProtocolInterpreter interpreter, INetworkStrategy networkStrategy) : base(networkStrategy)
        {
            this._bufferedPackages = new List<ByteArray>();
            this._backupPackages = new List<ByteArray>();
            this._interpreter = interpreter;
        }

        private void Connector_OnConnected()
        {
            this._reactor = this._connecter.Reactor;
            this._reactor.OnIOError += new IOErrorDelegate(this.Reactor_OnIOError);
            this._reactor.OnNetworkError += new NetworkErrorDelegate(this.Reactor_OnNetworkError);
            this._reactor.OnReceive += new HandleReadBufferDelegate(this.OnReceive);
            this._reactor.Start();
            if (this.OnConnected != null)
            {
                this.OnConnected();
            }
        }

        private void Connector_OnConnectFail(NetworkException networkError)
        {
            Logger.LogError("Fail to connect: {0}", new object[] { networkError.Message });
            if (this.OnConnectFail != null)
            {
                this.OnConnectFail(networkError);
            }
        }

        protected override bool InitializeImpl()
        {
            this._connecter = base.NetworkStrategy.CreateConnector();
            this._connecter.OnConnected += new NetworkEventDelegate(this.Connector_OnConnected);
            this._connecter.OnConnectFail += new NetworkErrorDelegate(this.Connector_OnConnectFail);
            this._connecter.Connect();
            return true;
        }

        private bool OnReceive(ByteArray byteArray)
        {
            if (base.Buffered)
            {
                this._bufferedPackages.Add(byteArray);
                return true;
            }
            return ((this._interpreter != null) && this._interpreter.InterpretMessage(0, byteArray));
        }

        public void ProcessSessionBuffers()
        {
            List<ByteArray> bufferPackages = this.BufferPackages;
            int num = 0;
            foreach (ByteArray array in bufferPackages)
            {
                if (this._interpreter.InterpretMessage(0, array))
                {
                    num++;
                }
            }
            if (bufferPackages.Count != num)
            {
                Logger.LogWarn("Some messages cannot be interpreted: {0}", new object[] { bufferPackages.Count - num });
            }
            bufferPackages.Clear();
        }

        private void Reactor_OnIOError(IOException ioError)
        {
            Logger.LogError("Reatcor IOError: {0}", new object[] { ioError.Message });
            if (this._reactor != null)
            {
                this._reactor.Stop();
                this._reactor = null;
                if (this.OnDisconnected != null)
                {
                    this.OnDisconnected();
                }
            }
        }

        private void Reactor_OnNetworkError(NetworkException networkError)
        {
            Logger.LogError("Reactor NetworkError: {0}", new object[] { networkError.Message });
            if (this._reactor != null)
            {
                this._reactor.Stop();
                this._reactor = null;
                if (this.OnDisconnected != null)
                {
                    this.OnDisconnected();
                }
            }
        }

        public void Send(ByteArray byteArray)
        {
            if (this._reactor != null)
            {
                this._reactor.Send(byteArray);
            }
        }

        protected override void StopImpl()
        {
            if (this._connecter != null)
            {
                this._connecter.Stop();
                this._connecter = null;
            }
            if (this._reactor != null)
            {
                this._reactor.Stop();
                this._reactor = null;
            }
        }

        protected override void ThreadLoopImpl()
        {
        }

        protected override void UpdateImpl()
        {
            if (this._connecter != null)
            {
                this._connecter.Update();
            }
            if (this._reactor != null)
            {
                this._reactor.Update();
            }
        }

        public List<ByteArray> BufferPackages
        {
            get
            {
                this._bufferedPackages = Interlocked.Exchange<List<ByteArray>>(ref this._backupPackages, this._bufferedPackages);
                return this._backupPackages;
            }
        }
    }
}

