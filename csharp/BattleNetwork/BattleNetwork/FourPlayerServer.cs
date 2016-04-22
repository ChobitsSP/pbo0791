namespace PokemonBattle.BattleNetwork
{
    using NetworkLib;
    using NetworkLib.Tcp;
    using NetworkLib.Utilities;
    using PokemonBattle.FourPlayer.Server;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class FourPlayerServer : NetworkServer, IProtocolInterpreter, IFourPlayerServerService
    {
        private object _locker = new object();
        private Dictionary<byte, string> _positions = new Dictionary<byte, string>();

        public event SetPositionDelegate OnPositionSet;

        public FourPlayerServer(string serverPlayer)
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 0x275a;
            strategy.Sync = true;
            base.NetworkStrategy = strategy;
            base._interpreter = this;
            base.OnClientDisconnected += new SessionDisconnectedDelegate(this.FourPlayerServer_OnClientDisconnected);
            this._positions[1] = serverPlayer;
            for (byte i = 2; i < 5; i = (byte) (i + 1))
            {
                this._positions[i] = "";
            }
        }

        protected override ClientSession CreateClientSession(int sessionID, IReactor reactor)
        {
            return new FourPlayerSession(sessionID, reactor, base.Buffered);
        }

        private void FourPlayerServer_OnClientDisconnected(ClientSession client)
        {
            lock (this._locker)
            {
                FourPlayerSession session = client as FourPlayerSession;
                if (session.Position != 0)
                {
                    this.SetPosition(session.Position, "");
                }
            }
        }

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return FourPlayerServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        public void OnClose(int sessionID)
        {
        }

        public void OnLogon(int sessionID, int identity)
        {
            lock (this._locker)
            {
                for (byte i = 1; i < 5; i = (byte) (i + 1))
                {
                    base.Send(sessionID, FourPlayerServerHelper.SetPosition(i, this._positions[i]));
                }
            }
        }

        public void OnSetPosition(int sessionID, byte position, string player)
        {
            lock (this._locker)
            {
                if (this._positions[position] == "")
                {
                    FourPlayerSession client = base.GetClient(sessionID) as FourPlayerSession;
                    if (client.Position != 0)
                    {
                        this.SetPosition(client.Position, "");
                    }
                    this.SetPosition(position, player);
                    client.Position = position;
                    base.Send(sessionID, FourPlayerServerHelper.SetPositionSuccess(position));
                }
            }
        }

        public void OnStartBattle(int sessionID)
        {
        }

        private void SetPosition(byte position, string player)
        {
            this._positions[position] = player;
            base.BroadCast(FourPlayerServerHelper.SetPosition(position, player));
            if (this.OnPositionSet != null)
            {
                this.OnPositionSet(position, player);
            }
        }

        public void StartBattle()
        {
            base.BroadCast(FourPlayerServerHelper.StartBattle(0));
        }
    }
}

