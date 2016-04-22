namespace PokemonBattle.FourPlayer.Server
{
    using System;

    public interface IFourPlayerServerService
    {
        void OnClose(int sessionID);
        void OnLogon(int sessionID, int identity);
        void OnSetPosition(int sessionID, byte position, string player);
        void OnStartBattle(int sessionID);
    }
}

