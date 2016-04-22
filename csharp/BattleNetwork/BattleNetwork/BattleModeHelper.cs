namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public static class BattleModeHelper
    {
        public static BattleMode Parse(int val)
        {
            if (val == 0x49944f53)
            {
                return BattleMode.Single;
            }
            if (val == 0x49a4342f)
            {
                return BattleMode.Double;
            }
            if (val == 0x56844ca5)
            {
                return BattleMode.Double_4P;
            }
            return BattleMode.WrongInput;
        }

        public static BattleMode ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleMode value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

