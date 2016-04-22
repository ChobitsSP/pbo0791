namespace NetworkLib
{
    using NetworkLib.Utilities;
    using System;
    using System.Net;
    using System.Runtime.CompilerServices;

    public interface IReactor : INetworkWorker
    {
        event IOErrorDelegate OnIOError;

        event NetworkErrorDelegate OnNetworkError;

        event HandleReadBufferDelegate OnReceive;

        void Send(ByteArray byteArray);

        IPAddress RemoteAddress { get; }
    }
}

