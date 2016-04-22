namespace NetworkLib.Utilities
{
    using System;

    public interface ILogManager
    {
        void Error(string message);
        void Info(string message);
        void Warn(string message);
    }
}

