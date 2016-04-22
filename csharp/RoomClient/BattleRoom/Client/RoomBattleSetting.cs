namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using System;

    public class RoomBattleSetting
    {
        public bool DoubleBan;
        public bool FourPlayerBan;
        public int MoveInterval;
        public bool RandomOnly;
        public bool SingleBan;
        public string Version;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.MoveInterval = byteArray.ReadInt();
            this.SingleBan = byteArray.ReadBoolean();
            this.DoubleBan = byteArray.ReadBoolean();
            this.FourPlayerBan = byteArray.ReadBoolean();
            this.RandomOnly = byteArray.ReadBoolean();
            this.Version = byteArray.ReadUTF();
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(this.MoveInterval);
            byteArray.WriteBoolean(this.SingleBan);
            byteArray.WriteBoolean(this.DoubleBan);
            byteArray.WriteBoolean(this.FourPlayerBan);
            byteArray.WriteBoolean(this.RandomOnly);
            byteArray.WriteUTF(this.Version);
        }
    }
}

