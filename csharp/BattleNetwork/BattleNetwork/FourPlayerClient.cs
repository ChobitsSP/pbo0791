namespace PokemonBattle.BattleNetwork
{
    using NetworkLib;
    using NetworkLib.Tcp;
    using NetworkLib.Utilities;
    using PokemonBattle.FourPlayer.Client;
    using System;
    using System.Threading;

    public class FourPlayerClient : NetworkClient, IProtocolInterpreter, IFourPlayerClientService
    {
        public event IdentityDelegate OnBattleReady;

        public event MyPositionDelegate OnMyPositionSet;

        public event SetPositionDelegate OnPositionSet;

        public event VoidFunctionDelegate OnServerClosed;

        public FourPlayerClient(string serverIP)
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 0x275a;
            strategy.Sync = true;
            strategy.ServerIP = serverIP;
            base.NetworkStrategy = strategy;
            base._interpreter = this;
        }

        public void Close()
        {
            base.Send(FourPlayerClientHelper.Close());
        }

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return FourPlayerClientHelper.InterpretMessage(sessionID, byteArray, this);
        }

        public void Logon(int identity)
        {
            base.Send(FourPlayerClientHelper.Logon(identity));
        }

        public void OnClose()
        {
            if (this.OnServerClosed != null)
            {
                this.OnServerClosed();
            }
        }

        public void OnSetPosition(byte position, string player)
        {
            if (this.OnPositionSet != null)
            {
                this.OnPositionSet(position, player);
            }
        }

        public void OnSetPositionSuccess(byte position)
        {
            if (this.OnMyPositionSet != null)
            {
                this.OnMyPositionSet(position);
            }
        }

        public void OnStartBattle(int identity)
        {
            if (this.OnBattleReady != null)
            {
                this.OnBattleReady(identity);
            }
        }

        public void SetPort(int port)
        {
            (base.NetworkStrategy as TcpNetworkStrategy).Port = port;
        }

        public void SetPosition(byte position, string player)
        {
            base.Send(FourPlayerClientHelper.SetPosition(position, player));
        }

        public void StartBattle()
        {
            base.Send(FourPlayerClientHelper.StartBattle());
        }
    }
}

