namespace PokemonBattle.BattleNetwork
{
    using NetworkLib;
    using System;

    internal class FourPlayerSession : ClientSession
    {
        private byte _position;

        public FourPlayerSession(int sessionID, IReactor reactor, bool buffered) : base(sessionID, reactor, buffered)
        {
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
    }
}

