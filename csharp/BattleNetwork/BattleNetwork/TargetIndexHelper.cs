namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public static class TargetIndexHelper
    {
        public static TargetIndex Parse(int val)
        {
            if (val == -1865455426)
            {
                return TargetIndex.DefaultTarget;
            }
            if (val == -536926594)
            {
                return TargetIndex.Opponent1;
            }
            if (val == -536926597)
            {
                return TargetIndex.Opponent2;
            }
            if (val == -1044722981)
            {
                return TargetIndex.TeamFriend;
            }
            if (val == -710105077)
            {
                return TargetIndex.Self;
            }
            if (val == -353152975)
            {
                return TargetIndex.Random;
            }
            return TargetIndex.WrongInput;
        }

        public static TargetIndex ReadFromByteArray(ByteArray byteArray)
        {
            return Parse(byteArray.ReadInt());
        }

        public static void WriteToByteArray(ByteArray byteArray, TargetIndex value)
        {
            byteArray.WriteInt((int) value);
        }
    }
}

