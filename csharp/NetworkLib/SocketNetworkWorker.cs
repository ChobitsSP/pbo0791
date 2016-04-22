namespace NetworkLib
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Threading;

    public abstract class SocketNetworkWorker : INetworkWorker, IDisposable
    {
        protected List<AsyncBlock> _asyncBlocks = new List<AsyncBlock>();
        protected List<AsyncBlock> _asyncBlockWaiter = new List<AsyncBlock>();
        protected byte[] _buffer = new byte[0x400];
        protected List<Exception> _errorBuffer = new List<Exception>();
        protected Exception _lastError;
        protected ByteArray _receiveBuffer = new ByteArray();
        protected System.Net.Sockets.Socket _socket;
        protected bool _sync;
        public const int OPERATION_ACCPET = 1;
        public const int OPERATION_CONNECT = 2;
        public const int OPERATION_READ_DIAGRAM = 3;
        public const int OPERATION_READ_STREAM = 3;
        public const int OPERATION_WRITE_DIAGRAM = 4;
        public const int OPERATION_WRITE_STREAM = 4;

        public SocketNetworkWorker(System.Net.Sockets.Socket socket, bool sync)
        {
            this._socket = socket;
            this._sync = sync;
        }

        protected void AddAsyncBlock(IAsyncResult asyncResult, System.Net.Sockets.Socket socket, int flag)
        {
            AsyncBlock item = new AsyncBlock(asyncResult, socket, flag);
            lock (this._asyncBlocks)
            {
                this._asyncBlocks.Add(item);
            }
        }

        protected abstract bool CompleteOperation(AsyncBlock var);
        protected void DelayError(Exception error)
        {
            this._lastError = error;
            this._errorBuffer.Add(error);
            if (this._errorBuffer.Count > 100)
            {
                this._errorBuffer.RemoveRange(0, 100);
                Logger.LogWarn("To much error in buffer, remove some to free up space", new object[0]);
            }
        }

        public void Dispose()
        {
            this.Stop();
        }

        private int ProcessAsyncBlocks()
        {
            this._asyncBlocks = Interlocked.Exchange<List<AsyncBlock>>(ref this._asyncBlockWaiter, this._asyncBlocks);
            List<AsyncBlock> list = new List<AsyncBlock>(this._asyncBlockWaiter);
            int num = 0;
            foreach (AsyncBlock block in list)
            {
                if (block._asyncResult.IsCompleted)
                {
                    try
                    {
                        if (!this.CompleteOperation(block))
                        {
                            Logger.LogWarn("Operation cannot be done. Flag {0}", new object[] { block._operationFlag });
                        }
                    }
                    catch (Exception exception)
                    {
                        this.DelayError(exception);
                    }
                    num++;
                    this._asyncBlockWaiter.Remove(block);
                }
            }
            return num;
        }

        public abstract bool Start();
        public virtual bool Stop()
        {
            if (this._socket != null)
            {
                try
                {
                    this._socket.Close();
                }
                catch (SocketException exception)
                {
                    Logger.LogError("Fail to stop socket worker: {0}", new object[] { exception.Message });
                }
                finally
                {
                    this._socket = null;
                }
            }
            return true;
        }

        public void Update()
        {
            int num = 0;
            while (this.ProcessAsyncBlocks() > 0)
            {
                num++;
                if (num == 10)
                {
                    Logger.LogWarn("Worker is very busy,probably too much workload: {0}", new object[] { num });
                }
                else
                {
                    if (num == 50)
                    {
                        Logger.LogWarn("Worker is extremely busy,too much workload or application error: {0}", new object[] { num });
                        continue;
                    }
                    if (num == 100)
                    {
                        Logger.LogWarn("Worker is terribly busy,quit loop to free some processor: {0}", new object[] { num });
                        break;
                    }
                }
            }
            List<Exception> list = new List<Exception>(this._errorBuffer);
            foreach (Exception exception in list)
            {
                Logger.LogError("Error in update: {0}", new object[] { exception.Message });
                this._errorBuffer.Remove(exception);
            }
        }

        protected List<Exception> ErrorBuffer
        {
            get
            {
                return this._errorBuffer;
            }
        }

        public Exception LastError
        {
            get
            {
                return this._lastError;
            }
        }

        public ByteArray ReceiveBuffer
        {
            get
            {
                return this._receiveBuffer;
            }
        }

        protected System.Net.Sockets.Socket Socket
        {
            get
            {
                return this._socket;
            }
        }

        protected class AsyncBlock
        {
            public IAsyncResult _asyncResult;
            public int _operationFlag;
            public Socket _socket;

            public AsyncBlock(IAsyncResult asyncResult, Socket socket, int flag)
            {
                this._asyncResult = asyncResult;
                this._socket = socket;
                this._operationFlag = flag;
            }
        }
    }
}

