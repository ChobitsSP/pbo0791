namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public static class BattleRuleHelper
    {
        public static BattleRule Parse(int val)
        {
            if (val == 0x90ce471)
            {
                return BattleRule.PPUp;
            }
            if (val == -353152975)
            {
                return BattleRule.Random;
            }
            return BattleRule.WrongInput;
        }

        public static BattleRule ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleRule value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

