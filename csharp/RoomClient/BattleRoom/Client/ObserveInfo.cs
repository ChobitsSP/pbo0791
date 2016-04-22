namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using System;

    public class ObserveInfo
    {
        public string BattleAddress;
        public int BattleIdentity;
        public byte Position;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.BattleAddress = byteArray.ReadUTF();
            this.BattleIdentity = byteArray.ReadInt();
            this.Position = byteArray.ReadByte();
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteUTF(this.BattleAddress);
            byteArray.WriteInt(this.BattleIdentity);
            byteArray.WriteByte(this.Position);
        }
    }
}

