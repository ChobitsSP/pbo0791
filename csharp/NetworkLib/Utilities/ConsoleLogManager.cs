namespace NetworkLib.Utilities
{
    using System;

    public sealed class ConsoleLogManager : ILogManager
    {
        public void Error(string message)
        {
            Console.WriteLine("{0}:Error:" + message, DateTime.Now.ToString("T"));
        }

        public void Info(string message)
        {
            Console.WriteLine("{0}:Info:" + message, DateTime.Now.ToString("T"));
        }

        public void Warn(string message)
        {
            Console.WriteLine("{0}:Warn:" + message, DateTime.Now.ToString("T"));
        }
    }
}

