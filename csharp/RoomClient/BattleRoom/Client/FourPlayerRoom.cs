namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using System;

    public class FourPlayerRoom
    {
        public int Identity;
        public string Name;
        public byte PlayerCount;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.Name = byteArray.ReadUTF();
            this.Identity = byteArray.ReadInt();
            this.PlayerCount = byteArray.ReadByte();
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteUTF(this.Name);
            byteArray.WriteInt(this.Identity);
            byteArray.WriteByte(this.PlayerCount);
        }
    }
}

