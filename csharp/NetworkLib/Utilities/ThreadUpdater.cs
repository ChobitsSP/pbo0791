namespace NetworkLib.Utilities
{
    using NetworkLib;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public sealed class ThreadUpdater
    {
        private ManualResetEvent _stopEvent;
        private Thread _thread;

        public event VoidFunctionDelegate OnDispose;

        public event VoidFunctionDelegate OnInitialize;

        public event IntFunctionDelegate OnUpdate;

        private void EventLoop()
        {
            try
            {
                if (this.OnInitialize != null)
                {
                    this.OnInitialize();
                }
                do
                {
                    int num = 0;
                    if (this.OnUpdate != null)
                    {
                        num = this.OnUpdate();
                    }
                    Thread.Sleep(Math.Max(num, 3));
                }
                while (!this._stopEvent.WaitOne(0, false));
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                this._thread = null;
                if (this.OnDispose != null)
                {
                    this.OnDispose();
                }
            }
        }

        public void Run()
        {
            this._stopEvent = new ManualResetEvent(false);
            this._thread = new Thread(new ThreadStart(this.EventLoop));
            this._thread.Start();
        }

        public void Stop()
        {
            this._stopEvent.Set();
        }
    }
}

