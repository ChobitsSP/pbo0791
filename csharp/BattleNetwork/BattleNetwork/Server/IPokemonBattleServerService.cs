namespace PokemonBattle.BattleNetwork.Server
{
    using PokemonBattle.BattleNetwork;
    using System;

    public interface IPokemonBattleServerService
    {
        void OnExit(int sessionID, string identity);
        void OnLogon(int sessionID, string identity, BattleMode modeInfo, string versionInfo);
        void OnReceiveBattleInfo(int sessionID, BattleInfo info);
        void OnReceiveBattleSnapshot(int sessionID, BattleSnapshot snapshot);
        void OnReceiveMove(int sessionID, PlayerMove move);
        void OnReceiveTeam(int sessionID, byte position, string identity, ByteSequence team);
        void OnReceiveTieMessage(int sessionID, string identity, TieMessage message);
        void OnRegistObsever(int sessionID, int identity);
        void OnTimeUp(int sessionID, string identity);
    }
}

