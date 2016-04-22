namespace NetworkLib
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IConnector : INetworkWorker
    {
        event NetworkEventDelegate OnConnected;

        event NetworkErrorDelegate OnConnectFail;

        void Connect();

        bool Connected { get; }

        IReactor Reactor { get; }
    }
}

