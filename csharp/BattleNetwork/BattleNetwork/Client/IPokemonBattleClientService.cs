namespace PokemonBattle.BattleNetwork.Client
{
    using PokemonBattle.BattleNetwork;
    using System;

    public interface IPokemonBattleClientService
    {
        void OnExit(string identity);
        void OnLogonFail(string message);
        void OnLogonSuccess();
        void OnReceiveBattleInfo(BattleInfo info);
        void OnReceiveBattleSnapshot(BattleSnapshot snapshot);
        void OnReceiveMove(PlayerMove move);
        void OnReceiveRandomSeed(int seed);
        void OnReceiveRules(BattleRuleSequence rules);
        void OnReceiveTeam(byte position, string identity, ByteSequence team);
        void OnReceiveTieMessage(string identity, TieMessage message);
        void OnRegistObsever(int identity);
        void OnTimeUp(string identity);
    }
}

