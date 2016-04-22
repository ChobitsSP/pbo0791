namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public static class TieMessageHelper
    {
        public static TieMessage Parse(int val)
        {
            if (val == -1115504277)
            {
                return TieMessage.TieRequest;
            }
            if (val == -803423805)
            {
                return TieMessage.AgreeTie;
            }
            if (val == -1091278321)
            {
                return TieMessage.RefuseTie;
            }
            if (val == 0x5fac4a27)
            {
                return TieMessage.Fail;
            }
            return TieMessage.WrongInput;
        }

        public static TieMessage ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, TieMessage value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

