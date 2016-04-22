namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public static class PokemonIndexHelper
    {
        public static PokemonIndex Parse(int val)
        {
            if (val == 0x7ff5d535)
            {
                return PokemonIndex.Pokemon1OfTeam1;
            }
            if (val == 0x3b42d535)
            {
                return PokemonIndex.Pokemon2OfTeam1;
            }
            if (val == 0x229d49d0)
            {
                return PokemonIndex.Pokemon1OfTeam2;
            }
            if (val == -571848240)
            {
                return PokemonIndex.Pokemon2OfTeam2;
            }
            return PokemonIndex.WrongInput;
        }

        public static PokemonIndex ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, PokemonIndex value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

