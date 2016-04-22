namespace NetworkLib
{
    using System;

    public interface INetworkWorker
    {
        bool Start();
        bool Stop();
        void Update();

        Exception LastError { get; }
    }
}

