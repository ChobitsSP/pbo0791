namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using System;

    public class User
    {
        public string Address;
        public string CustomDataHash;
        public string CustomDataInfo;
        public int Identity;
        public byte ImageKey;
        public string Name;
        public UserState State;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.Identity = byteArray.ReadInt();
            this.Name = byteArray.ReadUTF();
            this.State = UserStateHelper.ReadFromByteArray(byteArray);
            this.Address = byteArray.ReadUTF();
            this.ImageKey = byteArray.ReadByte();
            this.CustomDataInfo = byteArray.ReadUTF();
            this.CustomDataHash = byteArray.ReadUTF();
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(this.Identity);
            byteArray.WriteUTF(this.Name);
            UserStateHelper.WriteToByteArray(byteArray, this.State);
            byteArray.WriteUTF(this.Address);
            byteArray.WriteByte(this.ImageKey);
            byteArray.WriteUTF(this.CustomDataInfo);
            byteArray.WriteUTF(this.CustomDataHash);
        }
    }
}

