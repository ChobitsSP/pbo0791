namespace NetworkLib.Utilities
{
    using NetworkLib;
    using System;
    using System.Runtime.InteropServices;

    public sealed class PerformanceTimer
    {
        private long _frequency;
        private VoidFunctionDelegate _reset;
        private long _startTicks;
        private DoubleFunctionDelegate _timing;

        [DllImport("kernel32.dll")]
        public static extern short QueryPerformanceCounter(ref long x);
        [DllImport("kernel32.dll")]
        public static extern short QueryPerformanceFrequency(ref long x);
        public void ResetCounter()
        {
            this._reset();
        }

        private void ResetHighFreqTimer()
        {
            QueryPerformanceCounter(ref this._startTicks);
        }

        private void ResetLowFreqTimer()
        {
            this._startTicks = Environment.TickCount;
        }

        public void Start()
        {
            if (this.SupportHighFreqTimer())
            {
                QueryPerformanceFrequency(ref this._frequency);
                this._reset = new VoidFunctionDelegate(this.ResetHighFreqTimer);
                this._timing = new DoubleFunctionDelegate(this.TimingHighFreqTimer);
            }
            else
            {
                this._reset = new VoidFunctionDelegate(this.ResetLowFreqTimer);
                this._timing = new DoubleFunctionDelegate(this.TimingLowFreqTimer);
            }
            this._reset();
        }

        private bool SupportHighFreqTimer()
        {
            return (QueryPerformanceCounter(ref this._startTicks) != 0);
        }

        private double TimingHighFreqTimer()
        {
            long x = 0L;
            QueryPerformanceCounter(ref x);
            return (((x - this._startTicks) * 1000.0) / ((double) this._frequency));
        }

        private double TimingLowFreqTimer()
        {
            return (double) (Environment.TickCount - this._startTicks);
        }

        public double TimingMilliseconds
        {
            get
            {
                return this._timing();
            }
        }
    }
}

