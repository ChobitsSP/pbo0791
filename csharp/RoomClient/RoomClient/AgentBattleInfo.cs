namespace PokemonBattle.RoomClient
{
    using PokemonBattle.BattleNetwork;
    using System;

    public class AgentBattleInfo
    {
        private int _agentID;
        private PokemonBattle.BattleNetwork.BattleMode _battleMode;
        private int _moveInterval;
        private byte _position;
        private string _serverAddress;
        private string _userName;

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

        public PokemonBattle.BattleNetwork.BattleMode BattleMode
        {
            get
            {
                return this._battleMode;
            }
            set
            {
                this._battleMode = value;
            }
        }

        public int MoveInterval
        {
            get
            {
                return this._moveInterval;
            }
            set
            {
                this._moveInterval = value;
            }
        }

        public byte Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        public string ServerAddress
        {
            get
            {
                return this._serverAddress;
            }
            set
            {
                this._serverAddress = value;
            }
        }

        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }
    }
}

