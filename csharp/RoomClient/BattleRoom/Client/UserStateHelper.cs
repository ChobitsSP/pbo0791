namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using System;

    public static class UserStateHelper
    {
        public static UserState Parse(int val)
        {
            if (val == 0x1907c16b)
            {
                return UserState.Free;
            }
            if (val == 0xd402782)
            {
                return UserState.Challenging;
            }
            if (val == -1069528294)
            {
                return UserState.Battling;
            }
            if (val == -848236796)
            {
                return UserState.Away;
            }
            return UserState.WrongInput;
        }

        public static UserState ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, UserState value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

