namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using System;

    public static class BattleLinkModeHelper
    {
        public static BattleLinkMode Parse(int val)
        {
            if (val == -1063922077)
            {
                return BattleLinkMode.Direct;
            }
            if (val == -1597211022)
            {
                return BattleLinkMode.Agent;
            }
            return BattleLinkMode.WrongInput;
        }

        public static BattleLinkMode ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleLinkMode value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

