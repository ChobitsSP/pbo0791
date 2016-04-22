namespace NetworkLib.Udp
{
    using NetworkLib;
    using System;
    using System.Collections;

    public class UdpNetworkStrategy : INetworkStrategy
    {
        private int _port;
        private string _serverIP = string.Empty;
        private bool _sync = true;
        public const string SERVER_IP = "server_ip";
        public const string SERVER_PORT = "server_port";

        public IAcceptor CreateAcceptor()
        {
            return UdpAcceptor.Create(this._sync, this._port);
        }

        public IConnector CreateConnector()
        {
            return new UdpConnector(this._serverIP, this._port, this._sync);
        }

        public void Initialize(Hashtable param)
        {
            if (param.ContainsKey("server_ip"))
            {
                this._serverIP = (string) param["server_ip"];
            }
            if (param.ContainsKey("server_port"))
            {
                this._port = (int) param["server_port"];
            }
        }

        public int Port
        {
            get
            {
                return this._port;
            }
            set
            {
                this._port = value;
            }
        }

        public string ServerIP
        {
            get
            {
                return this._serverIP;
            }
            set
            {
                this._serverIP = value;
            }
        }

        public bool Sync
        {
            get
            {
                return this._sync;
            }
            set
            {
                this._sync = value;
            }
        }
    }
}

