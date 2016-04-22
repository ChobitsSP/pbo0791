namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using PokemonBattle.BattleNetwork;
    using System;

    public class ChallengeInfo
    {
        public PokemonBattle.BattleNetwork.BattleMode BattleMode;
        public BattleLinkMode LinkMode;
        public BattleRuleSequence Rules;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.BattleMode = BattleModeHelper.ReadFromByteArray(byteArray);
            this.LinkMode = BattleLinkModeHelper.ReadFromByteArray(byteArray);
            this.Rules = new BattleRuleSequence();
            this.Rules.ReadFromByteArray(byteArray);
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            BattleModeHelper.WriteToByteArray(byteArray, this.BattleMode);
            BattleLinkModeHelper.WriteToByteArray(byteArray, this.LinkMode);
            this.Rules.WriteToByteArray(byteArray);
        }
    }
}

