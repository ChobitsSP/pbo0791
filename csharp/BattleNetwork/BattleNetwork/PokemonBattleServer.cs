namespace PokemonBattle.BattleNetwork
{
    using NetworkLib;
    using NetworkLib.Tcp;
    using NetworkLib.Utilities;
    using PokemonBattle.BattleNetwork.Server;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class PokemonBattleServer : NetworkServer, IProtocolInterpreter, IPokemonBattleServerService
    {
        private List<int> _observerList = new List<int>();
        private Dictionary<int, string> _playerList = new Dictionary<int, string>();

        public event ClientConnectedDelegate OnBattleClientConnected;

        public event MessageDelegate OnPlayerExit;

        public event RequestBattleInfoDelegate OnRequestBattleInfo;

        public event SetMoveDelegate OnSetMove;

        public event SetTeamDelegate OnSetTeam;

        public event ServerTieDelegate OnTie;

        public PokemonBattleServer()
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 0x2757;
            strategy.Sync = true;
            base.NetworkStrategy = strategy;
            base._interpreter = this;
            base.OnClientDisconnected += new SessionDisconnectedDelegate(this.PokemonBattleServer_OnClientDisconnected);
        }

        public void Exit(string identity)
        {
            foreach (int num in this._playerList.Keys)
            {
                base.Send(num, PokemonBattleServerHelper.Exit(identity));
            }
        }

        private BattleInfo HandleRequestBattleInfoEvent()
        {
            if (this.OnRequestBattleInfo != null)
            {
                return this.OnRequestBattleInfo();
            }
            return null;
        }

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return PokemonBattleServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        public void OnExit(int sessionID, string identity)
        {
            if (this.OnPlayerExit != null)
            {
                this.OnPlayerExit(identity);
            }
        }

        public void OnLogon(int sessionID, string identity, BattleMode modeInfo, string versionInfo)
        {
            string str;
            if (this.VerifyClient(identity, modeInfo, versionInfo, out str))
            {
                base.Send(sessionID, PokemonBattleServerHelper.LogonSuccess());
                this._playerList[sessionID] = identity;
            }
            else
            {
                base.Send(sessionID, PokemonBattleServerHelper.LogonFail(str));
                base.Disconnect(sessionID);
            }
        }

        public void OnReceiveBattleInfo(int sessionID, BattleInfo info)
        {
        }

        public void OnReceiveBattleSnapshot(int sessionID, BattleSnapshot snapshot)
        {
        }

        public void OnReceiveMove(int sessionID, PlayerMove move)
        {
            if (this.OnSetMove != null)
            {
                this.OnSetMove(move);
            }
        }

        public void OnReceiveTeam(int sessionID, byte position, string identity, ByteSequence team)
        {
            if (this.OnSetTeam != null)
            {
                this.OnSetTeam(position, identity, team);
            }
        }

        public void OnReceiveTieMessage(int sessionID, string identity, TieMessage message)
        {
            if (this.OnTie != null)
            {
                this.OnTie(sessionID, identity, message);
            }
        }

        public void OnRegistObsever(int sessionID, int identity)
        {
            BattleInfo info = this.HandleRequestBattleInfoEvent();
            if (info != null)
            {
                base.Send(sessionID, PokemonBattleServerHelper.ReceiveBattleInfo(info));
            }
            this._observerList.Add(sessionID);
        }

        public void OnTimeUp(int sessionID, string identity)
        {
        }

        private void PokemonBattleServer_OnClientDisconnected(ClientSession client)
        {
            if (this._playerList.ContainsKey(client.SessionID))
            {
                this.OnExit(client.SessionID, this._playerList[client.SessionID]);
            }
            else if (this._observerList.Contains(client.SessionID))
            {
                this._observerList.Remove(client.SessionID);
            }
        }

        public void SendBattleInfo(BattleInfo info)
        {
            foreach (int num in this.ObserverList)
            {
                base.Send(num, PokemonBattleServerHelper.ReceiveBattleInfo(info));
            }
        }

        public void SendBattleSnapshot(BattleSnapshot snapshot)
        {
            foreach (int num in this.ObserverList)
            {
                base.Send(num, PokemonBattleServerHelper.ReceiveBattleSnapshot(snapshot));
            }
        }

        public void SendMove(PlayerMove move)
        {
            foreach (int num in this._playerList.Keys)
            {
                base.Send(num, PokemonBattleServerHelper.ReceiveMove(move));
            }
        }

        public void SendRandomSeed(int seed)
        {
            foreach (int num in this._playerList.Keys)
            {
                base.Send(num, PokemonBattleServerHelper.ReceiveRandomSeed(seed));
            }
        }

        public void SendRules(BattleRuleSequence rules)
        {
            foreach (int num in this._playerList.Keys)
            {
                base.Send(num, PokemonBattleServerHelper.ReceiveRules(rules));
            }
        }

        public void SendTeam(byte position, string identity, ByteSequence team)
        {
            foreach (int num in this._playerList.Keys)
            {
                base.Send(num, PokemonBattleServerHelper.ReceiveTeam(position, identity, team));
            }
        }

        public void Tie(string identity, TieMessage message)
        {
            foreach (int num in this._playerList.Keys)
            {
                base.Send(num, PokemonBattleServerHelper.ReceiveTieMessage(identity, message));
            }
        }

        public void TieExcept(int sessionID, string identity, TieMessage message)
        {
            foreach (int num in this._playerList.Keys)
            {
                if (num != sessionID)
                {
                    base.Send(num, PokemonBattleServerHelper.ReceiveTieMessage(identity, message));
                }
            }
        }

        public void TieRequestFail(int sessionID)
        {
            base.Send(sessionID, PokemonBattleServerHelper.ReceiveTieMessage("", TieMessage.Fail));
        }

        private bool VerifyClient(string identity, BattleMode modeInfo, string versionInfo, out string message)
        {
            if (this.OnBattleClientConnected != null)
            {
                return this.OnBattleClientConnected(identity, modeInfo, versionInfo, out message);
            }
            message = "";
            return true;
        }

        private List<int> ObserverList
        {
            get
            {
                return new List<int>(this._observerList);
            }
        }
    }
}

