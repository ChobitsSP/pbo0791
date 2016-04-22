namespace NetworkLib
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IAcceptor : INetworkWorker
    {
        event AcceptReactorDelegate OnAcceptNew;

        void OnAccept(IReactor reactor);
    }
}

