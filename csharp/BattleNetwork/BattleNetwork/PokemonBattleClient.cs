namespace PokemonBattle.BattleNetwork
{
    using NetworkLib;
    using NetworkLib.Tcp;
    using NetworkLib.Utilities;
    using PokemonBattle.BattleNetwork.Client;
    using System;
    using System.Threading;

    public class PokemonBattleClient : NetworkClient, IProtocolInterpreter, IPokemonBattleClientService
    {
        public event VoidFunctionDelegate OnLogoned;

        public event MessageDelegate OnLogonFailed;

        public event MessageDelegate OnPlayerExit;

        public event MessageDelegate OnPlayerTimeUp;

        public event VoidFunctionDelegate OnRegistObserver;

        public event BattleInfoDelegate OnSetBattleInfo;

        public event SetMoveDelegate OnSetMove;

        public event SetRulesDelegate OnSetRules;

        public event SetRandomSeedDelegate OnSetSeed;

        public event SnapshotDelegate OnSetSnapshot;

        public event SetTeamDelegate OnSetTeam;

        public event TieDelegate OnTie;

        public PokemonBattleClient(string serverIP)
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.ServerIP = serverIP;
            strategy.Port = 0x2757;
            strategy.Sync = true;
            base.NetworkStrategy = strategy;
            base._interpreter = this;
        }

        public void Exit(string identity)
        {
            base.Send(PokemonBattleClientHelper.Exit(identity));
        }

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return PokemonBattleClientHelper.InterpretMessage(sessionID, byteArray, this);
        }

        public void Logon(string identity, BattleMode mode, string versionInfo)
        {
            base.Send(PokemonBattleClientHelper.Logon(identity, mode, versionInfo));
        }

        public void OnExit(string identity)
        {
            if (this.OnPlayerExit != null)
            {
                this.OnPlayerExit(identity);
            }
        }

        public void OnLogonFail(string message)
        {
            if (this.OnLogonFailed != null)
            {
                this.OnLogonFailed(message);
            }
        }

        public void OnLogonSuccess()
        {
            if (this.OnLogoned != null)
            {
                this.OnLogoned();
            }
        }

        public void OnReceiveBattleInfo(BattleInfo info)
        {
            if (this.OnSetBattleInfo != null)
            {
                this.OnSetBattleInfo(info);
            }
        }

        public void OnReceiveBattleSnapshot(BattleSnapshot snapshot)
        {
            if (this.OnSetSnapshot != null)
            {
                this.OnSetSnapshot(snapshot);
            }
        }

        public void OnReceiveMove(PlayerMove move)
        {
            if (this.OnSetMove != null)
            {
                this.OnSetMove(move);
            }
        }

        public void OnReceiveRandomSeed(int seed)
        {
            if (this.OnSetSeed != null)
            {
                this.OnSetSeed(seed);
            }
        }

        public void OnReceiveRules(BattleRuleSequence rules)
        {
            if (this.OnSetRules != null)
            {
                this.OnSetRules(rules);
            }
        }

        public void OnReceiveTeam(byte position, string identity, ByteSequence team)
        {
            if (this.OnSetTeam != null)
            {
                this.OnSetTeam(position, identity, team);
            }
        }

        public void OnReceiveTieMessage(string identity, TieMessage message)
        {
            if (this.OnTie != null)
            {
                this.OnTie(identity, message);
            }
        }

        public void OnRegistObsever(int identity)
        {
            if (this.OnRegistObserver != null)
            {
                this.OnRegistObserver();
            }
        }

        public void OnTimeUp(string identity)
        {
            if (this.OnPlayerTimeUp != null)
            {
                this.OnPlayerTimeUp(identity);
            }
        }

        public void RegistObserver(int identity)
        {
            base.Send(PokemonBattleClientHelper.RegistObsever(identity));
        }

        public void SendBattleInfo(BattleInfo info)
        {
            base.Send(PokemonBattleClientHelper.ReceiveBattleInfo(info));
        }

        public void SendBattleSnapshot(BattleSnapshot snapshot)
        {
            base.Send(PokemonBattleClientHelper.ReceiveBattleSnapshot(snapshot));
        }

        public void SendMove(PlayerMove move)
        {
            base.Send(PokemonBattleClientHelper.ReceiveMove(move));
        }

        public void SendTeam(byte position, string identity, ByteSequence team)
        {
            base.Send(PokemonBattleClientHelper.ReceiveTeam(position, identity, team));
        }

        public void SetPort(int port)
        {
            (base.NetworkStrategy as TcpNetworkStrategy).Port = port;
        }

        public void Tie(string identity, TieMessage message)
        {
            base.Send(PokemonBattleClientHelper.ReceiveTieMessage(identity, message));
        }

        public void TimeUp(string identity)
        {
            base.Send(PokemonBattleClientHelper.TimeUp(identity));
        }
    }
}

