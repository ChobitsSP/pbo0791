namespace NetworkLib
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Tcp;
    public class NetworkServer : NetworkWorker
    {
        private IAcceptor _acceptor;
        private Dictionary<int, ClientSession> _clients;
        private Dictionary<int, ClientSession> _disconnectedClients;
        protected IProtocolInterpreter _interpreter;
        private int _sessionBase;

        public event SessionDisconnectedDelegate OnClientDisconnected;

        protected NetworkServer() : this(null, new TcpNetworkStrategy())
        {
        }

        protected NetworkServer(INetworkStrategy networkStrategy) : this(null, networkStrategy)
        {
        }

        public NetworkServer(IProtocolInterpreter interpreter) : this(interpreter, new TcpNetworkStrategy())
        {
        }

        public NetworkServer(IProtocolInterpreter interpreter, INetworkStrategy networkStrategy) : base(networkStrategy)
        {
            this._clients = new Dictionary<int, ClientSession>();
            this._disconnectedClients = new Dictionary<int, ClientSession>();
            this._interpreter = interpreter;
        }

        private void Acceptor_OnAcceptNew(IReactor reactor)
        {
            ClientSession session = this.CreateClientSession(this._sessionBase, reactor);
            session.OnReceive += new SessionReceivedDelegate(this.HandlePackages);
            session.OnSessionDisconnected += new SessionDisconnectedDelegate(this.ClientDisconnected);
            this._clients[this._sessionBase] = session;
            Interlocked.Increment(ref this._sessionBase);
        }

        public void BroadCast(ByteArray byteArray)
        {
            foreach (ClientSession session in this.Sessions)
            {
                session.Send(byteArray);
            }
        }

        private void ClientDisconnected(ClientSession client)
        {
            this._disconnectedClients[client.SessionID] = client;
            this.HandleClientDisconnectedEvent(client);
        }

        protected virtual ClientSession CreateClientSession(int sessionID, IReactor reactor)
        {
            return new ClientSession(sessionID, reactor, false);
        }

        public bool Disconnect(int sessionID)
        {
            ClientSession client = this.GetClient(sessionID);
            if (client != null)
            {
                client.Stop();
                this.ClientDisconnected(client);
                return true;
            }
            return false;
        }

        public ClientSession GetClient(int sessionID)
        {
            ClientSession session;
            if (this._clients.TryGetValue(sessionID, out session))
            {
                return session;
            }
            return null;
        }

        private void HandleClientDisconnectedEvent(ClientSession client)
        {
            if (this.OnClientDisconnected != null)
            {
                this.OnClientDisconnected(client);
            }
        }

        private bool HandlePackages(ClientSession client, ByteArray byteArray)
        {
            return ((this._interpreter != null) && this._interpreter.InterpretMessage(client.SessionID, byteArray));
        }

        protected override bool InitializeImpl()
        {
            this._acceptor = base.NetworkStrategy.CreateAcceptor();
            if (this._acceptor == null)
            {
                Logger.LogError("Fail to create acceptor!", new object[0]);
                return false;
            }
            this._acceptor.OnAcceptNew += new AcceptReactorDelegate(this.Acceptor_OnAcceptNew);
            this._acceptor.Start();
            return true;
        }

        public void ProcessSessionBuffers()
        {
            foreach (ClientSession session in this.Sessions)
            {
                this.ProcessSessionBuffers(session, session.BufferPackages);
            }
        }

        private int ProcessSessionBuffers(ClientSession client, List<ByteArray> buffers)
        {
            int num = 0;
            foreach (ByteArray array in buffers)
            {
                if (this.HandlePackages(client, array))
                {
                    num++;
                }
            }
            if (buffers.Count != num)
            {
                Logger.LogWarn("{0} out of {1} processed.", new object[] { num, buffers.Count });
            }
            buffers.Clear();
            return num;
        }

        public bool Send(int sessionID, ByteArray byteArray)
        {
            ClientSession client = this.GetClient(sessionID);
            if (client != null)
            {
                client.Send(byteArray);
                return true;
            }
            return false;
        }

        protected override void StopImpl()
        {
            if (this._acceptor != null)
            {
                this._acceptor.Stop();
                this._acceptor = null;
            }
            foreach (ClientSession session in this.Sessions)
            {
                session.Stop();
            }
        }

        protected override void ThreadLoopImpl()
        {
        }

        protected override void UpdateImpl()
        {
            if (this._acceptor != null)
            {
                this._acceptor.Update();
            }
            foreach (ClientSession session in this.Sessions)
            {
                session.Update();
            }
            Dictionary<int, ClientSession> dictionary = new Dictionary<int, ClientSession>();
            foreach (int num in Interlocked.Exchange<Dictionary<int, ClientSession>>(ref dictionary, this._disconnectedClients).Keys)
            {
                this._clients.Remove(num);
            }
        }

        private List<ClientSession> Sessions
        {
            get
            {
                return new List<ClientSession>(this._clients.Values);
            }
        }
    }
}

