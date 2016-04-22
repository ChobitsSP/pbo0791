namespace PokemonBattle.BattleNetwork
{
    using NetworkLib;
    using System;

    public class BattleAgentSession : ClientSession
    {
        private int _agentID;

        public BattleAgentSession(int sessionID, IReactor reactor, bool buffered) : base(sessionID, reactor, buffered)
        {
            this._agentID = -1;
        }

        public int AgentID
        {
            get
            {
                return this._agentID;
            }
            set
            {
                this._agentID = value;
            }
        }
    }
}

