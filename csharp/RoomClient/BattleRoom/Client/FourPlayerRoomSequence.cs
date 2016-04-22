namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;

    public class FourPlayerRoomSequence
    {
        private List<FourPlayerRoom> _elements = new List<FourPlayerRoom>();

        public void ReadFromByteArray(ByteArray byteArray)
        {
            int num = byteArray.ReadInt();
            for (int i = 0; i < num; i++)
            {
                FourPlayerRoom item = new FourPlayerRoom();
                item.ReadFromByteArray(byteArray);
                this._elements.Add(item);
            }
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(this._elements.Count);
            foreach (FourPlayerRoom room in this._elements)
            {
                room.WriteToByteArray(byteArray);
            }
        }

        public List<FourPlayerRoom> Elements
        {
            get
            {
                return this._elements;
            }
        }
    }
}

