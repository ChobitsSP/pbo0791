namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;

    public class PokemonSnapshotSequence
    {
        private List<PokemonSnapshot> _elements = new List<PokemonSnapshot>();

        public void ReadFromByteArray(ByteArray byteArray)
        {
            int num = byteArray.ReadInt();
            for (int i = 0; i < num; i++)
            {
                PokemonSnapshot item = new PokemonSnapshot();
                item.ReadFromByteArray(byteArray);
                this._elements.Add(item);
            }
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(this._elements.Count);
            foreach (PokemonSnapshot snapshot in this._elements)
            {
                snapshot.WriteToByteArray(byteArray);
            }
        }

        public List<PokemonSnapshot> Elements
        {
            get
            {
                return this._elements;
            }
        }
    }
}

