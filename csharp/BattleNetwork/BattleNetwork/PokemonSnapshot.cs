namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public class PokemonSnapshot
    {
        public byte Gender;
        public bool Hid;
        public short Hp;
        public int Identity;
        public byte Lv;
        public short MaxHp;
        public string Nickname;
        public byte State;
        public bool Substituded;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.Identity = byteArray.ReadInt();
            this.Nickname = byteArray.ReadUTF();
            this.Hp = byteArray.ReadShort();
            this.MaxHp = byteArray.ReadShort();
            this.Gender = byteArray.ReadByte();
            this.Lv = byteArray.ReadByte();
            this.State = byteArray.ReadByte();
            this.Substituded = byteArray.ReadBoolean();
            this.Hid = byteArray.ReadBoolean();
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(this.Identity);
            byteArray.WriteUTF(this.Nickname);
            byteArray.WriteShort(this.Hp);
            byteArray.WriteShort(this.MaxHp);
            byteArray.WriteByte(this.Gender);
            byteArray.WriteByte(this.Lv);
            byteArray.WriteByte(this.State);
            byteArray.WriteBoolean(this.Substituded);
            byteArray.WriteBoolean(this.Hid);
        }
    }
}

