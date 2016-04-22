namespace PokemonBattle.FourPlayer.Client
{
    using System;

    public interface IFourPlayerClientService
    {
        void OnClose();
        void OnSetPosition(byte position, string player);
        void OnSetPositionSuccess(byte position);
        void OnStartBattle(int identity);
    }
}

