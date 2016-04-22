namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;
    using System.Collections.Generic;

    public class BattleRuleSequence
    {
        private List<BattleRule> _elements = new List<BattleRule>();

        public void ReadFromByteArray(ByteArray byteArray)
        {
            int num = byteArray.ReadInt();
            for (int i = 0; i < num; i++)
            {
                BattleRule item = BattleRuleHelper.ReadFromByteArray(byteArray);
                this._elements.Add(item);
            }
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(this._elements.Count);
            foreach (BattleRule rule in this._elements)
            {
                BattleRuleHelper.WriteToByteArray(byteArray, rule);
            }
        }

        public List<BattleRule> Elements
        {
            get
            {
                return this._elements;
            }
        }
    }
}

