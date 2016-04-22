namespace PokemonBattle.BattleNetwork
{
    using NetworkLib;
    using NetworkLib.Tcp;
    using NetworkLib.Utilities;
    using PokemonBattle.BattleNetwork.Server;
    using System;
    using System.Collections.Generic;

    public class BattleAgentServer : NetworkServer, IProtocolInterpreter, IPokemonBattleServerService
    {
        private Dictionary<int, BattleAgent> _agents = new Dictionary<int, BattleAgent>();
        private int _battleIdentityBase = -1;

        public BattleAgentServer()
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 0x2759;
            strategy.Sync = true;
            base.NetworkStrategy = strategy;
            base._interpreter = this;
            base.UpdateInterval = 500;
            base.OnLogicUpdate += new VoidFunctionDelegate(this.BattleAgentServer_OnLogicUpdate);
            base.OnClientDisconnected += new SessionDisconnectedDelegate(this.BattleAgentServer_OnClientDisconnected);
        }

        public int AddBattle(BattleMode mode, BattleRuleSequence rules)
        {
            this._battleIdentityBase++;
            BattleAgent agent = new BattleAgent(mode, rules.Elements, this);
            this._agents[this._battleIdentityBase] = agent;
            Logger.LogInfo("Add battle agent, ID : {0}", new object[] { this._battleIdentityBase });
            return this._battleIdentityBase;
        }

        private void BattleAgentServer_OnClientDisconnected(ClientSession client)
        {
            BattleAgentSession session = client as BattleAgentSession;
            if (session.AgentID != -1)
            {
                BattleAgent agent = this.GetAgent(session.AgentID);
                if ((agent != null) && !agent.BattleEnd)
                {
                    agent.PlayerExit(client.SessionID);
                }
            }
        }

        private void BattleAgentServer_OnLogicUpdate()
        {
            List<int> list = new List<int>(this._agents.Keys);
            foreach (int num in list)
            {
                if (this._agents[num].BattleEnd)
                {
                    this._agents.Remove(num);
                    Logger.LogInfo("Remove battle agent, ID : {0}", new object[] { num });
                }
            }
        }

        protected override ClientSession CreateClientSession(int sessionID, IReactor reactor)
        {
            return new BattleAgentSession(sessionID, reactor, base.Buffered);
        }

        private BattleAgent GetAgent(int agentID)
        {
            BattleAgent agent;
            if (this._agents.TryGetValue(agentID, out agent))
            {
                return agent;
            }
            return null;
        }

        private BattleAgent GetClientAgent(int sessionID)
        {
            ClientSession client = base.GetClient(sessionID);
            if ((client != null) && ((client as BattleAgentSession).AgentID != -1))
            {
                return this.GetAgent((client as BattleAgentSession).AgentID);
            }
            return null;
        }

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return PokemonBattleServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        public void OnExit(int sessionID, string identity)
        {
            base.Disconnect(sessionID);
        }

        public void OnLogon(int sessionID, string identity, BattleMode modeInfo, string versionInfo)
        {
            int num;
            BattleAgent agent = null;
            if (int.TryParse(identity, out num))
            {
                agent = this.GetAgent(num);
            }
            if (agent != null)
            {
                if (!agent.BattleEnd)
                {
                    (base.GetClient(sessionID) as BattleAgentSession).AgentID = num;
                    agent.UserLogon(sessionID, identity);
                    base.Send(sessionID, PokemonBattleServerHelper.LogonSuccess());
                }
                else
                {
                    base.Send(sessionID, PokemonBattleServerHelper.LogonFail("对手已退出"));
                    base.Disconnect(sessionID);
                }
            }
            else
            {
                base.Send(sessionID, PokemonBattleServerHelper.LogonFail("无法找到对应的对战代理"));
                base.Disconnect(sessionID);
            }
        }

        public void OnReceiveBattleInfo(int sessionID, BattleInfo info)
        {
            BattleAgent clientAgent = this.GetClientAgent(sessionID);
            if (clientAgent != null)
            {
                clientAgent.ReceiveBattleInfo(info);
            }
        }

        public void OnReceiveBattleSnapshot(int sessionID, BattleSnapshot snapshot)
        {
            BattleAgent clientAgent = this.GetClientAgent(sessionID);
            if (clientAgent != null)
            {
                clientAgent.ReceiveBattleSnapshot(snapshot);
            }
        }

        public void OnReceiveMove(int sessionID, PlayerMove move)
        {
            BattleAgent clientAgent = this.GetClientAgent(sessionID);
            if (clientAgent != null)
            {
                clientAgent.ReceiveMove(sessionID, move);
            }
        }

        public void OnReceiveTeam(int sessionID, byte position, string identity, ByteSequence team)
        {
            BattleAgent clientAgent = this.GetClientAgent(sessionID);
            if (clientAgent != null)
            {
                clientAgent.ReceiveTeam(sessionID, position, identity, team);
            }
        }

        public void OnReceiveTieMessage(int sessionID, string identity, TieMessage message)
        {
            BattleAgent clientAgent = this.GetClientAgent(sessionID);
            if (clientAgent != null)
            {
                clientAgent.ReceiveTieMessage(sessionID, message);
            }
        }

        public void OnRegistObsever(int sessionID, int identity)
        {
            BattleAgent agent = this.GetAgent(identity);
            if ((agent != null) && !agent.BattleEnd)
            {
                agent.RegistObserver(sessionID);
            }
        }

        public void OnTimeUp(int sessionID, string identity)
        {
            BattleAgent clientAgent = this.GetClientAgent(sessionID);
            if (clientAgent != null)
            {
                clientAgent.PlayerTimeUp(sessionID, identity);
            }
        }

        internal void PlayerTimeUp(int sessionID, string identity)
        {
            base.Send(sessionID, PokemonBattleServerHelper.TimeUp(identity));
        }

        internal void RegistObserver(int sessionID)
        {
            base.Send(sessionID, PokemonBattleServerHelper.RegistObsever(0));
        }

        internal void SendBattleData(int sessionID, List<BattleRule> rules, int randomSeed)
        {
            BattleRuleSequence sequence = new BattleRuleSequence();
            sequence.Elements.AddRange(rules);
            base.Send(sessionID, PokemonBattleServerHelper.ReceiveRules(sequence));
            base.Send(sessionID, PokemonBattleServerHelper.ReceiveRandomSeed(randomSeed));
        }

        internal void SendBattleInfo(int sessionID, BattleInfo info)
        {
            base.Send(sessionID, PokemonBattleServerHelper.ReceiveBattleInfo(info));
        }

        internal void SendBattleSnapshot(int sessionID, BattleSnapshot snapshot)
        {
            base.Send(sessionID, PokemonBattleServerHelper.ReceiveBattleSnapshot(snapshot));
        }

        internal void SendExitMessage(int sessionID, string identity)
        {
            base.Send(sessionID, PokemonBattleServerHelper.Exit(identity));
        }

        internal void SendMove(int sessionID, PlayerMove move)
        {
            base.Send(sessionID, PokemonBattleServerHelper.ReceiveMove(move));
        }

        internal void SendTeam(int sessionID, byte position, string identity, ByteSequence byteSequence)
        {
            base.Send(sessionID, PokemonBattleServerHelper.ReceiveTeam(position, identity, byteSequence));
        }

        internal void SendTieMessage(int sessionID, string identity, TieMessage message)
        {
            base.Send(sessionID, PokemonBattleServerHelper.ReceiveTieMessage(identity, message));
        }
    }
}

