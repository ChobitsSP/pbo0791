namespace PokemonBattle.BattleNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal class BattleAgent
    {
        private int _agreeTieCounter;
        private bool _battleEnd;
        private BattleInfo _battleInfo;
        private BattleMode _battleMode;
        private object _locker = new object();
        private List<int> _observers = new List<int>();
        private int _randomSeed;
        private List<BattleRule> _rules;
        private BattleAgentServer _server;
        private bool _tieing;
        private Dictionary<int, UserInfo> _users = new Dictionary<int, UserInfo>();
        private int _waitUserCount;
        private int observeHost;

        public BattleAgent(BattleMode mode, List<BattleRule> rules, BattleAgentServer server)
        {
            this._server = server;
            this._battleMode = mode;
            this._rules = rules;
            this._randomSeed = new Random().Next();
            if (mode == BattleMode.Double_4P)
            {
                this._waitUserCount = 4;
            }
            else
            {
                this._waitUserCount = 2;
            }
        }

        private List<int> GetObserverList()
        {
            return new List<int>(this._observers);
        }

        public void PlayerExit(int identity)
        {
            if (this._users.ContainsKey(identity))
            {
                foreach (int num in this._users.Keys)
                {
                    if (identity != num)
                    {
                        this._server.SendExitMessage(num, this._users[identity].Name);
                    }
                }
                List<int> observerList = this.GetObserverList();
                if (identity == this.observeHost)
                {
                    foreach (int num2 in observerList)
                    {
                        this._server.SendExitMessage(num2, this._users[identity].Name);
                    }
                }
                this._battleEnd = true;
            }
        }

        public void PlayerTimeUp(int identity, string player)
        {
            if (this._users.ContainsKey(identity))
            {
                foreach (int num in this._users.Keys)
                {
                    this._server.PlayerTimeUp(num, player);
                }
            }
        }

        public void ReceiveBattleInfo(BattleInfo info)
        {
            this._battleInfo = info;
            foreach (int num in this.GetObserverList())
            {
                this._server.SendBattleInfo(num, info);
            }
        }

        public void ReceiveBattleSnapshot(BattleSnapshot snapshot)
        {
            foreach (int num in this.GetObserverList())
            {
                this._server.SendBattleSnapshot(num, snapshot);
            }
        }

        public void ReceiveMove(int identity, PlayerMove move)
        {
            if (this._users.ContainsKey(identity))
            {
                foreach (int num in this._users.Keys)
                {
                    this._server.SendMove(num, move);
                }
            }
        }

        public void ReceiveTeam(int identity, byte position, string name, ByteSequence teamData)
        {
            if (this._users.ContainsKey(identity))
            {
                this._users[identity].Name = name;
                this._users[identity].TeamData = teamData;
                this._users[identity].Position = position;
                lock (this._locker)
                {
                    Interlocked.Decrement(ref this._waitUserCount);
                    if (this._waitUserCount == 0)
                    {
                        this.SendData();
                    }
                }
            }
        }

        public void ReceiveTieMessage(int identity, TieMessage message)
        {
            if (this._users.ContainsKey(identity))
            {
                if (message == TieMessage.TieRequest)
                {
                    if (this._tieing)
                    {
                        this._server.SendTieMessage(identity, "", TieMessage.Fail);
                        return;
                    }
                    this._tieing = true;
                }
                if (this._tieing)
                {
                    if (message == TieMessage.AgreeTie)
                    {
                        this._agreeTieCounter++;
                        if ((this._agreeTieCounter + 1) != this._users.Count)
                        {
                            return;
                        }
                    }
                    else if (message == TieMessage.RefuseTie)
                    {
                        this._tieing = false;
                        this._agreeTieCounter = 0;
                    }
                    foreach (int num in this._users.Keys)
                    {
                        if ((message == TieMessage.AgreeTie) || (num != identity))
                        {
                            this._server.SendTieMessage(num, this._users[identity].Name, message);
                        }
                    }
                }
            }
        }

        public void RegistObserver(int identity)
        {
            this._observers.Add(identity);
            if (this._battleInfo != null)
            {
                this._server.SendBattleInfo(identity, this._battleInfo);
            }
        }

        private void SendData()
        {
            foreach (int num in this._users.Keys)
            {
                foreach (int num2 in this._users.Keys)
                {
                    if (num != num2)
                    {
                        this._server.SendTeam(num, this._users[num2].Position, this._users[num2].Name, this._users[num2].TeamData);
                    }
                }
            }
            foreach (int num3 in this._users.Keys)
            {
                this._server.SendBattleData(num3, this._rules, this._randomSeed);
            }
            this.observeHost = new List<int>(this._users.Keys)[0];
            this._server.RegistObserver(this.observeHost);
        }

        public void UserLogon(int identity, string name)
        {
            if (!this._users.ContainsKey(identity))
            {
                lock (this._locker)
                {
                    if (this._waitUserCount == 0)
                    {
                        return;
                    }
                }
                UserInfo info = new UserInfo(name, 0, null);
                this._users[identity] = info;
            }
        }

        public bool BattleEnd
        {
            get
            {
                return this._battleEnd;
            }
        }

        private class UserInfo
        {
            private string _name;
            private byte _position;
            private ByteSequence _teamData;

            public UserInfo(string name, byte position, ByteSequence teamData)
            {
                this._name = name;
                this._position = position;
                this._teamData = teamData;
            }

            public string Name
            {
                get
                {
                    return this._name;
                }
                set
                {
                    this._name = value;
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

            public ByteSequence TeamData
            {
                get
                {
                    return this._teamData;
                }
                set
                {
                    this._teamData = value;
                }
            }
        }
    }
}

