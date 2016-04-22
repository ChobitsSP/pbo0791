namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public static class BattleMoveHelper
    {
        public static BattleMove Parse(int val)
        {
            if (val == 0x33c8c03e)
            {
                return BattleMove.Attack;
            }
            if (val == -762866655)
            {
                return BattleMove.SwapPokemon;
            }
            if (val == -532471963)
            {
                return BattleMove.DeathSwap;
            }
            if (val == -1116926315)
            {
                return BattleMove.PassSwap;
            }
            return BattleMove.WrongInput;
        }

        public static BattleMove ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleMove value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

