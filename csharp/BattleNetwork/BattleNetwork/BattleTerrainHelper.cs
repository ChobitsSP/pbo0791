namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public static class BattleTerrainHelper
    {
        public static BattleTerrain Parse(int val)
        {
            if (val == -1444406344)
            {
                return BattleTerrain.Stadium;
            }
            if (val == 0x163aaac3)
            {
                return BattleTerrain.Grass;
            }
            if (val == -803934465)
            {
                return BattleTerrain.Flat;
            }
            if (val == 0x31c99141)
            {
                return BattleTerrain.Sand;
            }
            if (val == 0x3aad7499)
            {
                return BattleTerrain.Mountain;
            }
            if (val == -1859589031)
            {
                return BattleTerrain.Cave;
            }
            if (val == 0x4b0c0d3)
            {
                return BattleTerrain.Water;
            }
            if (val == -1106010300)
            {
                return BattleTerrain.SnowField;
            }
            return BattleTerrain.WrongInput;
        }

        public static BattleTerrain ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleTerrain value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

