namespace NetworkLib
{
    using NetworkLib.Utilities;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class NetworkWorker : IDisposable
    {
        private bool _buffered;
        private PerformanceTimer _frameTimer;
        private PerformanceTimer _logicTimer;
        private int _maxIdle = 5;
        private INetworkStrategy _networkStrategy;
        private ThreadUpdater _thread;
        private int _updateInterval;

        public event VoidFunctionDelegate OnLogicUpdate;

        protected NetworkWorker(INetworkStrategy networkStrategy)
        {
            this._networkStrategy = networkStrategy;
        }

        public void Dispose()
        {
            this.Stop();
        }

        public bool Initialize()
        {
            this._frameTimer = new PerformanceTimer();
            this._logicTimer = new PerformanceTimer();
            return this.InitializeImpl();
        }

        protected abstract bool InitializeImpl();
        public void RunThread()
        {
            this._frameTimer.Start();
            this._logicTimer.Start();
            this._thread = new ThreadUpdater();
            this._thread.OnInitialize += new VoidFunctionDelegate(this.ThreadLoopImpl);
            this._thread.OnUpdate += new IntFunctionDelegate(this.Update);
            this._thread.Run();
        }

        public void RunThread(int updateInterval, VoidFunctionDelegate logicUpdate)
        {
            this._updateInterval = updateInterval;
            this.OnLogicUpdate = (VoidFunctionDelegate) Delegate.Combine(this.OnLogicUpdate, logicUpdate);
            this.RunThread();
        }

        public void Stop()
        {
            if (this._thread != null)
            {
                this._thread.Stop();
                this._thread = null;
            }
            this.StopImpl();
        }

        protected abstract void StopImpl();
        protected abstract void ThreadLoopImpl();
        public int Update()
        {
            int timingMilliseconds = (int) this._frameTimer.TimingMilliseconds;
            this.UpdateImpl();
            if ((this._updateInterval > 0) && (this._logicTimer.TimingMilliseconds > this._updateInterval))
            {
                if (this.OnLogicUpdate != null)
                {
                    this.OnLogicUpdate();
                }
                this._logicTimer.ResetCounter();
            }
            this._frameTimer.ResetCounter();
            return (this._maxIdle - timingMilliseconds);
        }

        protected abstract void UpdateImpl();

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

        public int MaxIdle
        {
            get
            {
                return this._maxIdle;
            }
            set
            {
                this._maxIdle = value;
            }
        }

        protected INetworkStrategy NetworkStrategy
        {
            get
            {
                return this._networkStrategy;
            }
            set
            {
                this._networkStrategy = value;
            }
        }

        public int UpdateInterval
        {
            get
            {
                return this._updateInterval;
            }
            set
            {
                this._updateInterval = value;
            }
        }
    }
}

