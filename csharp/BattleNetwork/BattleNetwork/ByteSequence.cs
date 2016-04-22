namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;

    public class ByteSequence
    {
        private List<byte> _elements = new List<byte>();

        public void ReadFromByteArray(ByteArray byteArray)
        {
            int num = byteArray.ReadInt();
            for (int i = 0; i < num; i++)
            {
                byte item = byteArray.ReadByte();
                this._elements.Add(item);
            }
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(this._elements.Count);
            foreach (byte num in this._elements)
            {
                byteArray.WriteByte(num);
            }
        }

        public List<byte> Elements
        {
            get
            {
                return this._elements;
            }
        }
    }
}

