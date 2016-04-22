namespace PokemonBattle.BattleNetwork
{
    using System;
    using System.Collections.Generic;

    public class FourPlayerAgent
    {
        private bool _closed;
        private int _identity;
        private object _locker = new object();
        private Dictionary<int, byte> _players = new Dictionary<int, byte>();
        private Dictionary<byte, string> _positions = new Dictionary<byte, string>();
        private FourPlayerAgentServer _server;

        public FourPlayerAgent(int identity, FourPlayerAgentServer server, string hostName)
        {
            this._identity = identity;
            this._server = server;
            this._positions[1] = hostName;
            for (byte i = 2; i < 5; i = (byte) (i + 1))
            {
                this._positions[i] = "";
            }
        }

        internal void Close()
        {
            this._closed = true;
            foreach (int num in this.PlayerList)
            {
                this._server.Close(num);
            }
        }

        public byte GetPlayerCount()
        {
            byte num = 4;
            Dictionary<byte, string> positions = this.Positions;
            foreach (byte num2 in positions.Keys)
            {
                if (positions[num2] == "")
                {
                    num = (byte) (num - 1);
                }
            }
            return num;
        }

        internal void PlayerExit(int identity)
        {
            if (this._players[identity] != 0)
            {
                this.SetPosition(this._players[identity], "");
                this._players.Remove(identity);
                this._server.UpdateRoom(this._identity, this.GetPlayerCount());
            }
        }

        internal void PlayerLogon(int identity)
        {
            this._players[identity] = 0;
            Dictionary<byte, string> positions = this.Positions;
            foreach (byte num in positions.Keys)
            {
                this._server.SetPosition(identity, num, positions[num]);
            }
        }

        internal void SetPosition(byte position, string player)
        {
            this._positions[position] = player;
            foreach (int num in this.PlayerList)
            {
                this._server.SetPosition(num, position, player);
            }
        }

        internal void SetPosition(int identity, byte position, string player)
        {
            lock (this._locker)
            {
                if (this._positions[position] == "")
                {
                    this.SetPosition(position, player);
                    if (this._players[identity] != 0)
                    {
                        this.SetPosition(this._players[identity], "");
                    }
                    else
                    {
                        this._server.UpdateRoom(this._identity, this.GetPlayerCount());
                    }
                    this._players[identity] = position;
                    this._server.SetPositionSuccess(identity, position);
                }
            }
        }

        internal void StartBattle(int battleIdentity)
        {
            foreach (int num in this.PlayerList)
            {
                this._server.StartBattle(num, battleIdentity);
            }
            this._closed = true;
        }

        public int AgentID
        {
            get
            {
                return this._identity;
            }
        }

        public bool Closed
        {
            get
            {
                return this._closed;
            }
        }

        public string HostName
        {
            get
            {
                return this._positions[1];
            }
        }

        private List<int> PlayerList
        {
            get
            {
                return new List<int>(this._players.Keys);
            }
        }

        private Dictionary<byte, string> Positions
        {
            get
            {
                return new Dictionary<byte, string>(this._positions);
            }
        }
    }
}

