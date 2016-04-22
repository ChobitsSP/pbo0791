namespace NetworkLib
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ClientSession
    {
        private List<ByteArray> _backupPackages = new List<ByteArray>();
        private bool _buffered;
        private List<ByteArray> _bufferPackages = new List<ByteArray>();
        private DateTime _connectedTime = DateTime.Now;
        private IReactor _reactor;
        private int _sessionID;

        public event SessionReceivedDelegate OnReceive;

        public event SessionDisconnectedDelegate OnSessionDisconnected;

        public event SessionRemovingDelegate OnSessionRemoving;

        public ClientSession(int sessionID, IReactor reactor, bool buffered)
        {
            this._sessionID = sessionID;
            this._reactor = reactor;
            this._buffered = buffered;
            this._reactor.OnIOError += new IOErrorDelegate(this.Reactor_OnIOError);
            this._reactor.OnNetworkError += new NetworkErrorDelegate(this.Reactor_OnNetworkError);
            this._reactor.OnReceive += new HandleReadBufferDelegate(this.Reactor_OnReceive);
            this._reactor.Start();
        }

        private bool HandleReceiveEvent(ByteArray byteArray)
        {
            if (this.OnReceive != null)
            {
                return this.OnReceive(this, byteArray);
            }
            return true;
        }

        private void HandleSessionDisconnectedEvent()
        {
            if (this.OnSessionDisconnected != null)
            {
                this.OnSessionDisconnected(this);
            }
        }

        private bool HandleSessionRemovingEvent()
        {
            if (this.OnSessionRemoving != null)
            {
                return this.OnSessionRemoving(this);
            }
            return true;
        }

        public virtual void LogicUpdate()
        {
        }

        private void Reactor_OnIOError(IOException ioError)
        {
            if (this.HandleSessionRemovingEvent())
            {
                this._reactor.Stop();
                this._reactor = null;
                this.HandleSessionDisconnectedEvent();
            }
        }

        private void Reactor_OnNetworkError(NetworkException networkError)
        {
            if (this.HandleSessionRemovingEvent())
            {
                this._reactor.Stop();
                this._reactor = null;
                this.HandleSessionDisconnectedEvent();
            }
        }

        private bool Reactor_OnReceive(ByteArray byteArray)
        {
            if (this.Buffered)
            {
                this._bufferPackages.Add(byteArray);
                return true;
            }
            return this.HandleReceiveEvent(byteArray);
        }

        public void Send(ByteArray byteArray)
        {
            if (this._reactor != null)
            {
                this._reactor.Send(byteArray);
            }
        }

        public void Stop()
        {
            if (this._reactor != null)
            {
                this._reactor.Stop();
                this._reactor = null;
            }
        }

        public void Update()
        {
            if (this._reactor != null)
            {
                this._reactor.Update();
            }
        }

        public bool Buffered
        {
            get
            {
                return this._buffered;
            }
            set
            {
                this._buffered = value;
            }
        }

        public List<ByteArray> BufferPackages
        {
            get
            {
                this._bufferPackages = Interlocked.Exchange<List<ByteArray>>(ref this._backupPackages, this._bufferPackages);
                return this._backupPackages;
            }
        }

        public IPAddress ClientAddress
        {
            get
            {
                if (this._reactor != null)
                {
                    return this._reactor.RemoteAddress;
                }
                return null;
            }
        }

        public int SessionID
        {
            get
            {
                return this._sessionID;
            }
            set
            {
                this._sessionID = value;
            }
        }
    }
}

