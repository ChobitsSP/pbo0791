namespace NetworkLib.Utilities
{
    using System;
    using System.Diagnostics;

    public sealed class TraceLogManager : ILogManager
    {
        public TraceLogManager()
        {
            this.InitTrace();
        }

        public void Error(string message)
        {
            Trace.TraceError(message);
        }

        public void Info(string message)
        {
            Trace.TraceInformation(message);
        }

        private void InitTrace()
        {
            Trace.AutoFlush = true;
            ConsoleTraceListener listener = new ConsoleTraceListener();
            Trace.Listeners.Add(listener);
            DefaultTraceListener listener2 = new DefaultTraceListener();
            Trace.Listeners.Add(listener2);
        }

        public void Warn(string message)
        {
            Trace.TraceWarning(message);
        }
    }
}

