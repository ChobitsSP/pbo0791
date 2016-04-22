namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;

    public class UserSequence
    {
        private List<User> _elements = new List<User>();

        public void ReadFromByteArray(ByteArray byteArray)
        {
            int num = byteArray.ReadInt();
            for (int i = 0; i < num; i++)
            {
                User item = new User();
                item.ReadFromByteArray(byteArray);
                this._elements.Add(item);
            }
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(this._elements.Count);
            foreach (User user in this._elements)
            {
                user.WriteToByteArray(byteArray);
            }
        }

        public List<User> Elements
        {
            get
            {
                return this._elements;
            }
        }
    }
}

