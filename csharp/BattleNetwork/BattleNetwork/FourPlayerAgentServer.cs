namespace PokemonBattle.BattleNetwork
{
    using NetworkLib;
    using NetworkLib.Tcp;
    using NetworkLib.Utilities;
    using PokemonBattle.FourPlayer.Server;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class FourPlayerAgentServer : NetworkServer, IProtocolInterpreter, IFourPlayerServerService
    {
        private int _agentBase = -1;
        private Dictionary<int, FourPlayerAgent> _agents = new Dictionary<int, FourPlayerAgent>();

        public event IntFunctionDelegate OnAddBattle;

        public event RemoveDelegate OnRemoveRoom;

        public event UpdateCountDelegate OnUpdateRoom;

        public FourPlayerAgentServer()
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 0x275b;
            strategy.Sync = true;
            base.NetworkStrategy = strategy;
            base._interpreter = this;
            base.UpdateInterval = 500;
            base.OnClientDisconnected += new SessionDisconnectedDelegate(this.FourPlayerAgentServer_OnClientDisconnected);
            base.OnLogicUpdate += new VoidFunctionDelegate(this.FourPlayerAgentServer_OnLogicUpdate);
        }

        public int AddAgent(string hostName)
        {
            this._agentBase++;
            FourPlayerAgent agent = new FourPlayerAgent(this._agentBase, this, hostName);
            this._agents[this._agentBase] = agent;
            return this._agentBase;
        }

        internal void Close(int identity)
        {
            base.Send(identity, FourPlayerServerHelper.Close());
        }

        protected override ClientSession CreateClientSession(int sessionID, IReactor reactor)
        {
            return new BattleAgentSession(sessionID, reactor, base.Buffered);
        }

        private void FourPlayerAgentServer_OnClientDisconnected(ClientSession client)
        {
            BattleAgentSession session = client as BattleAgentSession;
            if (session.AgentID != -1)
            {
                FourPlayerAgent agent = this.GetAgent(session.AgentID);
                if ((agent != null) && !agent.Closed)
                {
                    agent.PlayerExit(client.SessionID);
                }
            }
        }

        private void FourPlayerAgentServer_OnLogicUpdate()
        {
            Dictionary<int, FourPlayerAgent> dictionary = new Dictionary<int, FourPlayerAgent>(this._agents);
            foreach (int num in dictionary.Keys)
            {
                if (dictionary[num].Closed)
                {
                    this._agents.Remove(num);
                    if (this.OnRemoveRoom != null)
                    {
                        this.OnRemoveRoom(num);
                    }
                }
            }
        }

        private FourPlayerAgent GetAgent(int agentID)
        {
            FourPlayerAgent agent;
            if (this._agents.TryGetValue(agentID, out agent))
            {
                return agent;
            }
            return null;
        }

        public List<FourPlayerAgent> GetAgents()
        {
            return new List<FourPlayerAgent>(this._agents.Values);
        }

        private FourPlayerAgent GetClientAgent(int sessionID)
        {
            ClientSession client = base.GetClient(sessionID);
            if ((client != null) && ((client as BattleAgentSession).AgentID != -1))
            {
                return this.GetAgent((client as BattleAgentSession).AgentID);
            }
            return null;
        }

        private int HandleAddBattleEvent()
        {
            if (this.OnAddBattle != null)
            {
                return this.OnAddBattle();
            }
            return -1;
        }

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return FourPlayerServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        public void OnClose(int sessionID)
        {
            FourPlayerAgent clientAgent = this.GetClientAgent(sessionID);
            if (clientAgent != null)
            {
                clientAgent.Close();
            }
        }

        public void OnLogon(int sessionID, int identity)
        {
            BattleAgentSession client = base.GetClient(sessionID) as BattleAgentSession;
            FourPlayerAgent agent = this.GetAgent(identity);
            if ((agent != null) && !agent.Closed)
            {
                client.AgentID = identity;
                agent.PlayerLogon(sessionID);
            }
        }

        public void OnSetPosition(int sessionID, byte position, string player)
        {
            FourPlayerAgent clientAgent = this.GetClientAgent(sessionID);
            if ((clientAgent != null) && !clientAgent.Closed)
            {
                clientAgent.SetPosition(sessionID, position, player);
            }
        }

        public void OnStartBattle(int sessionID)
        {
            FourPlayerAgent clientAgent = this.GetClientAgent(sessionID);
            if (clientAgent != null)
            {
                int battleIdentity = this.HandleAddBattleEvent();
                clientAgent.StartBattle(battleIdentity);
            }
        }

        internal void SetPosition(int sessionID, byte position, string player)
        {
            base.Send(sessionID, FourPlayerServerHelper.SetPosition(position, player));
        }

        internal void SetPositionSuccess(int sessionID, byte position)
        {
            base.Send(sessionID, FourPlayerServerHelper.SetPositionSuccess(position));
        }

        internal void StartBattle(int sessionID, int identity)
        {
            base.Send(sessionID, FourPlayerServerHelper.StartBattle(identity));
        }

        internal void UpdateRoom(int identity, byte count)
        {
            if (this.OnUpdateRoom != null)
            {
                this.OnUpdateRoom(identity, count);
            }
        }
    }
}

