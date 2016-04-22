namespace NetworkLib.Utilities
{
    using System;

    public class Logger
    {
        private static NetworkLib.Utilities.LogLevel _level = NetworkLib.Utilities.LogLevel.Warn;
        private static ILogManager _logManager;

        static Logger()
        {
            SetDefaultLogger();
        }

        private Logger()
        {
        }

        public static void LogError(string format, params object[] args)
        {
            _logManager.Error(string.Format(format, args));
        }

        public static void LogException(Exception exception)
        {
            LogError("Exception : {0}", new object[] { exception.Message });
        }

        public static void LogInfo(string format, params object[] args)
        {
            if (Level == NetworkLib.Utilities.LogLevel.Info)
            {
                _logManager.Info(string.Format(format, args));
            }
        }

        public static void LogWarn(string format, params object[] args)
        {
            if (Level != NetworkLib.Utilities.LogLevel.Error)
            {
                _logManager.Warn(string.Format(format, args));
            }
        }

        public static void SetDefaultLogger()
        {
            LogManager = new ConsoleLogManager();
        }

        public static void SetTraceLogger()
        {
            LogManager = new TraceLogManager();
        }

        public static NetworkLib.Utilities.LogLevel Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public static ILogManager LogManager
        {
            get
            {
                return _logManager;
            }
            set
            {
                if (value == null)
                {
                    LogWarn("Cannot set null log manager", new object[0]);
                }
                else
                {
                    _logManager = value;
                }
            }
        }
    }
}

