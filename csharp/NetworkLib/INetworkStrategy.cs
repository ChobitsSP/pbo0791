namespace NetworkLib
{
    using System;
    using System.Collections;

    public interface INetworkStrategy
    {
        IAcceptor CreateAcceptor();
        IConnector CreateConnector();
        void Initialize(Hashtable param);
    }
}

